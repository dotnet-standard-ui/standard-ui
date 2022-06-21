namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class WinUIUIFramework : XamlUIFramework
    {
        public WinUIUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => "Microsoft.StandardUI.WinUI";
        public override string RootNamespace => "Microsoft.StandardUI.WinUI";
        public override TypeName DependencyPropertyType => new("Microsoft.UI.Xaml", "DependencyProperty");
        public override TypeName ContentPropertyAttribute => new("Microsoft.UI.Xaml.Markup", "ContentPropertyAttribute");

        public override string FrameworkTypeForUIElementAttachedTarget => "Microsoft.UI.Xaml.FrameworkElement";
        public override string ToFrameworkTypeForUIElementAttachedTarget => "ToFrameworkElement";
        public override string NativeUIElementType => "Microsoft.UI.Xaml.FrameworkElement";
        public override string WrapperSuffix => "WinUI";
        protected override string FontFamilyDefaultValue => "FontFamilyExtensions.DefaultFontFamily";

        public override void GenerateAttributes(Interface intface, ClassSource classSource)
        {
            if (intface.ContentPropertyName != null)
            {
                classSource.Attributes.Usings.AddNamespace(ContentPropertyAttribute.Namespace);
                classSource.Attributes.AddLine($"[ContentProperty(Name = \"{intface.ContentPropertyName}\")]");
            }
        }

        public override void GenerateStandardPanelLayoutMethods(string layoutManagerTypeName, Source methods)
        {
            methods.AddBlankLineIfNonempty();
            methods.AddLine($"protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size constraint) =>");
            using (methods.Indent())
            {
                methods.AddLine(
                    $"{layoutManagerTypeName}.Instance.MeasureOverride(this, constraint.ToStandardUISize()).ToWindowsFoundationSize();");
            }

            methods.AddBlankLine();
            methods.AddLine($"protected override global::Windows.Foundation.Size ArrangeOverride(global::Windows.Foundation.Size arrangeSize) =>");
            using (methods.Indent())
            {
                methods.AddLine(
                    $"{layoutManagerTypeName}.Instance.ArrangeOverride(this, arrangeSize.ToStandardUISize()).ToWindowsFoundationSize();");
            }
        }

        public override void GeneratePanelMethods(Source methods)
        {
#if LATER
            methods.AddBlankLineIfNonempty();

            methods.AddLine(
                "protected override int VisualChildrenCount => _children.Count;");
            methods.AddBlankLine();
            methods.AddLine(
                "protected override System.Windows.Media.Visual GetVisualChild(int index) => (System.Windows.Media.Visual) _children[index];");
#endif
        }

        public override void GenerateDrawableObjectMethods(Interface intface, Source methods)
        {
            methods.AddBlankLineIfNonempty();
            methods.AddLine(
                $"public override void Draw(IDrawingContext drawingContext) => drawingContext.Draw{intface.FrameworkClassName}(this);");

            if (intface.IsThisType(KnownTypes.ITextBlock))
            {
                methods.AddLine(
                    $"protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size constraint) =>");
                using (methods.Indent())
                {
                    methods.AddLine(
                        $"HostEnvironment.VisualFramework.MeasureTextBlock(this).ToWindowsFoundationSize();");
                }
            }
        }
    }
}
