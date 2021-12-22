namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class WpfUIFramework : XamlUIFramework
    {
        public WpfUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => "StandardUI.Wpf";
        public override string RootNamespace => "Microsoft.StandardUI.Wpf";
        public override string DependencyPropertyClassName => "System.Windows.DependencyProperty";
        public override string FrameworkTypeForUIElementAttachedTarget => "System.Windows.UIElement";
        public override string? DefaultBaseClassName => "StandardUIDependencyObject";
        public override string BuiltInUIElementBaseClassName => "BuiltInUIElement";
        public override string WrapperSuffix => "Wpf";
        protected override string FontFamilyDefaultValue => "FontFamilyExtensions.DefaultFontFamily";

        public override void GenerateStandardPanelLayoutMethods(string layoutManagerTypeName, Source methods)
        {
            methods.AddBlankLineIfNonempty();
            methods.AddLine($"protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>");
            using (methods.Indent())
            {
                methods.AddLine(
                    $"{layoutManagerTypeName}.Instance.MeasureOverride(this, constraint.ToStandardUISize()).ToWpfSize();");
            }

            methods.AddBlankLine();
            methods.AddLine($"protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) =>");
            using (methods.Indent())
            {
                methods.AddLine(
                    $"{layoutManagerTypeName}.Instance.ArrangeOverride(this, arrangeSize.ToStandardUISize()).ToWpfSize();");
            }
        }

        public override void GeneratePanelMethods(Source methods)
        {
            methods.AddBlankLineIfNonempty();

            methods.AddLine(
                "protected override int VisualChildrenCount => _children.Count;");
            methods.AddBlankLine();
            methods.AddLine(
                "protected override System.Windows.Media.Visual GetVisualChild(int index) => (System.Windows.Media.Visual) _children[index];");
        }

        public override void GenerateDrawableObjectMethods(Interface intface, Source methods)
        {
            methods.AddBlankLineIfNonempty();
            methods.AddLine(
                $"public override void Draw(IDrawingContext drawingContext) => drawingContext.Draw{intface.FrameworkClassName}(this);");

            if (intface.IsThisType(KnownTypes.ITextBlock))
            {
                methods.AddLine(
                    $"protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>");
                using (methods.Indent())
                {
                    methods.AddLine(
                        $"StandardUIEnvironment.Instance.VisualEnvironment.MeasureTextBlock(this).ToWpfSize();");
                }
            }
        }
    }
}
