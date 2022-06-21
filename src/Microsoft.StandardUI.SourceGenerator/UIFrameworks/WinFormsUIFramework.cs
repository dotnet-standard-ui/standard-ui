namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
    public class WinFormsUIFramework : NonXamlUIFramework
    {
        public WinFormsUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => "Microsoft.StandardUI.WinForms";
        public override string RootNamespace => "Microsoft.StandardUI.WinForms";
        public override string FrameworkTypeForUIElementAttachedTarget => "System.Windows.Forms.Control";
        public override string NativeUIElementType => "System.Windows.Forms.Control";
        protected override string FontFamilyDefaultValue => "\"\""; // TODO: Supply right value here
    }
}
