namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class MacUIFramework : NonXamlUIFramework
    {
        public MacUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => "Microsoft.StandardUI.Mac";
        public override string RootNamespace => "Microsoft.StandardUI.Mac";
        public override string FrameworkTypeForUIElementAttachedTarget => "StandardUIElement";
        public override string NativeUIElementType => "AppKit.NSView";
        protected override string FontFamilyDefaultValue => "\"\""; // TODO: Supply right value here
    }
}
