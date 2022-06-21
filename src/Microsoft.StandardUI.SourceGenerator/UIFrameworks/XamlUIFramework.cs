using Microsoft.CodeAnalysis;

namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public abstract class XamlUIFramework : UIFramework
    {
        protected XamlUIFramework(Context context) : base(context)
        {
        }
        public abstract string ToFrameworkTypeForUIElementAttachedTarget { get; }
        public abstract TypeName DependencyPropertyType { get; }
        public abstract TypeName ContentPropertyAttribute { get; }

        public override string UIElementCollectionOutputTypeName(ITypeSymbol elementType) => $"UIElementCollection<{NativeUIElementType},{elementType}>";
        public override string UIElementSubtypeCollectionOutputTypeName(ITypeSymbol elementType) => $"UIElementCollection<{OutputTypeName(elementType)},{elementType.Name}>";

        public override void GenerateAttributes(Interface intface, ClassSource classSource)
        {
            if (intface.ContentPropertyName != null)
            {
                classSource.Attributes.Usings.AddNamespace(ContentPropertyAttribute.Namespace);
                classSource.Attributes.AddLine($"[ContentProperty(\"{intface.ContentPropertyName}\")]");
            }
        }

        public override void GenerateProperty(Property property, ClassSource classSource)
        {
            classSource.Usings.AddType(DependencyPropertyType);

            // Generate the property descriptor
            string nonNullablePropertyType = Utils.ToNonnullableType(PropertyOutputTypeName(property));
            string descriptorName = PropertyDescriptorName(property);
            string defaultValue = DefaultValue(property);
            classSource.StaticFields.AddLine(
                $"public static readonly {DependencyPropertyType.Name} {descriptorName} = PropertyUtils.Register(nameof({property.Name}), typeof({nonNullablePropertyType}), typeof({property.Interface.FrameworkClassName}), {defaultValue});");

            if (property.IsUICollection)
            {
                string fieldName = PropertyFieldName(property);
                string fieldTypeName = PropertyOutputTypeName(property);

                classSource.NonstaticFields.AddLine(
                    $"private {fieldTypeName} {fieldName};");

                classSource.DefaultConstructorBody.AddLines(
                    $"{fieldName} = new {fieldTypeName}(this);",
                    $"SetValue({descriptorName}, {fieldName});");
            }

            GeneratePropertyMethods(property, classSource.NonstaticMethods);
        }

        private void GeneratePropertyMethods(Property property, Source source)
        {
            var usings = source.Usings;
            string propertyOutputTypeName = PropertyOutputTypeName(property);

            // Add the type - for interface type and the framework type (if different)
            usings.AddTypeNamespace(property.Type);
            if (IsWrappedType(property.Type) || Utils.IsUIModelInterfaceType(property.Type))
                usings.AddNamespace(ToFrameworkNamespaceName(property.Type.ContainingNamespace));

            AddTypeAliasUsingIfNeeded(usings, propertyOutputTypeName);

            bool classPropertyTypeDiffersFromInterface = property.TypeName != propertyOutputTypeName;

#if LATER
            SyntaxTokenList modifiers;
            if (includeXmlComment)
                modifiers = TokenList(
                    Token(
                        TriviaList(xmlCommentTrivia),
                        SyntaxKind.PublicKeyword,
                        TriviaList()));
            else
                modifiers = TokenList(Token(SyntaxKind.PublicKeyword));
#endif

            source.AddBlankLineIfNonempty();
            string descriptorName = PropertyDescriptorName(property);

            string getterValue;
            if (property.IsUICollection)
                getterValue = $"{PropertyFieldName(property)}";
            else
                getterValue = $"({propertyOutputTypeName}) GetValue({descriptorName})";

            if (property.IsReadOnly)
                source.AddLine($"public {propertyOutputTypeName} {property.Name} => {getterValue};");
            else
            {
                source.AddLines(
                    $"public {propertyOutputTypeName} {property.Name}",
                    "{");
                using (source.Indent())
                {
                    source.AddLine(
                        $"get => {getterValue};");
                    source.AddLine(
                        $"set => SetValue({descriptorName}, value);");
                }
                source.AddLine(
                    "}");
            }

#if LATER
            //if (!includeXmlComment)
            propertyDeclaration = propertyDeclaration.WithLeadingTrivia(
                    TriviaList(propertyDeclaration.GetLeadingTrivia()
                        .Insert(0, CarriageReturnLineFeed)
                        .Insert(0, CarriageReturnLineFeed)));
#endif

            // If the interface property has a different type, add another property that explicitly implements it
            if (classPropertyTypeDiffersFromInterface)
            {
                string otherGetterValue;
                string setterAssignment;
                if (IsWrappedType(property.Type))
                {
                    otherGetterValue = $"{property.Name}.{property.TypeName}";
                    setterAssignment = $"{property.Name} = new {propertyOutputTypeName}(value)";
                }
                else if (Utils.IsUICollectionType(Context, property.Type, out var elementType) && propertyOutputTypeName.StartsWith("UIElementCollection<"))
                {
                    otherGetterValue = $"{property.Name}.ToStandardUIElementCollection()";
                    setterAssignment = ""; // Not used
                }
                else
                {
                    otherGetterValue = property.Name;
                    setterAssignment = $"{property.Name} = ({propertyOutputTypeName}) value";
                }

                if (property.IsReadOnly)
                {
                    source.AddLine(
                        $"{property.TypeName} {property.Interface.Name}.{property.Name} => {otherGetterValue};");
                }
                else
                {
                    source.AddLines(
                        $"{property.TypeName} {property.Interface.Name}.{property.Name}",
                        "{");
                    using (source.Indent())
                    {
                        source.AddLine(
                            $"get => {otherGetterValue};");
                        source.AddLine(
                            $"set => {setterAssignment};");
                    }
                    source.AddLine(
                        "}");
                }
            }
        }

        public override void GenerateAttachedProperty(AttachedProperty attachedProperty, ClassSource mainClassSource, ClassSource attachedClassSource)
        {
            mainClassSource.Usings.AddType(DependencyPropertyType);

            string parameterAsAttachedTargetType = Utils.IsThisType(attachedProperty.TargetType, KnownTypes.IUIElement) ?
                $"{attachedProperty.TargetParameterName}.{ToFrameworkTypeForUIElementAttachedTarget}()" :
                $"({OutputTypeName(attachedProperty.TargetType)}) {attachedProperty.TargetParameterName}";

            string targetOutputTypeName = AttachedTargetOutputTypeName(attachedProperty);
            string propertyOutputTypeName = PropertyOutputTypeName(attachedProperty);
            string nonNullablePropertyType = Utils.ToNonnullableType(propertyOutputTypeName);
            string descriptorName = PropertyDescriptorName(attachedProperty);
            string defaultValue = DefaultValue(attachedProperty);

            mainClassSource.StaticFields.AddLine(
                $"public static readonly {DependencyPropertyType.Name} {descriptorName} = PropertyUtils.RegisterAttached(\"{attachedProperty.Name}\", typeof({nonNullablePropertyType}), typeof({targetOutputTypeName}), {defaultValue});");

            mainClassSource.StaticMethods.AddBlankLineIfNonempty();
            mainClassSource.StaticMethods.AddLine($"public static {propertyOutputTypeName} Get{attachedProperty.Name}({targetOutputTypeName} {attachedProperty.TargetParameterName}) => ({propertyOutputTypeName}) {attachedProperty.TargetParameterName}.GetValue({descriptorName});");
            if (attachedProperty.SetterMethod != null)
                mainClassSource.StaticMethods.AddLine($"public static void Set{attachedProperty.Name}({targetOutputTypeName} {attachedProperty.TargetParameterName}, {propertyOutputTypeName} value) => {attachedProperty.TargetParameterName}.SetValue({descriptorName}, value);");

            attachedClassSource.NonstaticMethods.AddBlankLineIfNonempty();
            attachedClassSource.NonstaticMethods.AddLine($"public {propertyOutputTypeName} Get{attachedProperty.Name}({attachedProperty.TargetTypeName} {attachedProperty.TargetParameterName}) => {attachedProperty.Interface.FrameworkClassName}.Get{attachedProperty.Name}({parameterAsAttachedTargetType});");
            if (attachedProperty.SetterMethod != null)
                attachedClassSource.NonstaticMethods.AddLine($"public void Set{attachedProperty.Name}({attachedProperty.TargetTypeName} {attachedProperty.TargetParameterName}, {propertyOutputTypeName} value) => {attachedProperty.Interface.FrameworkClassName}.Set{attachedProperty.Name}({parameterAsAttachedTargetType}, value);");
        }

        public override bool IsWrappedType(ITypeSymbol type)
        {
            string typeName = type.Name;
            return typeName == "Color" || typeName == "Point" || typeName == "Points" || typeName == "Size" || typeName == "DataSource" || typeName == "FontWeight";
        }
    }
}
