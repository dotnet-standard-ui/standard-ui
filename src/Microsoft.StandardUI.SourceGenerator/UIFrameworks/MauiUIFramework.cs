namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class MauiUIFramework : XamlUIFramework
    {
        public MauiUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => "Microsoft.StandardUI.Maui";
        public override string RootNamespace => "Microsoft.StandardUI.Maui";
        public override TypeName DependencyPropertyType => new("Microsoft.Maui.Controls", "BindableProperty");
        public override TypeName ContentPropertyAttribute => new("Microsoft.Maui.Controls", "ContentPropertyAttribute");

        public override string FrameworkTypeForUIElementAttachedTarget => "Microsoft.Maui.Controls.View";
        public override string ToFrameworkTypeForUIElementAttachedTarget => "ToView";

        public override string NativeUIElementType => "Microsoft.Maui.Controls.View";
        public override string WrapperSuffix => "Maui";
        protected override string FontFamilyDefaultValue => "\"\""; // TODO: Supply right value here

        public override void AddTypeAliasUsingIfNeeded(Usings usings, string destinationTypeName)
        {
            // These types are also defined in Maui, so add aliases to prefer the Standard UI type
            if (destinationTypeName == "Brush" || destinationTypeName == "Brush?")
                usings.AddTypeAlias("Brush = Microsoft.StandardUI.Maui.Media.Brush");
            else if (destinationTypeName == "SweepDirection")
                usings.AddTypeAlias("SweepDirection = Microsoft.StandardUI.Media.SweepDirection");
        }
    }
}
