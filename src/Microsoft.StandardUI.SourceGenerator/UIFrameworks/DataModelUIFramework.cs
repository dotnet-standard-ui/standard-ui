using System.IO;

namespace Microsoft.StandardUI.SourceGenerator.UIFrameworks
{
#if LATER
    public class DataModelUIFramework : UIFramework
    {
        public DataModelUIFramework(Context context) : base(context)
        {
        }

        public override string ProjectBaseDirectory => Path.Combine("StandardUI", "DataModel");
        public override string RootNamespace => "Microsoft.StandardUI.DataModel";
        public override string FrameworkTypeForUIElementAttachedTarget => "ObjectWithCascadingNotifications";
        public override string NativeUIElementType => "ObjectWithCascadingNotifications";
        //public override string? DefaultBaseClassName => "ObjectWithCascadingNotifications";
        public override string BuiltInUIElementBaseClassName => "ObjectWithCascadingNotifications";
        protected override string FontFamilyDefaultValue => throw new System.NotImplementedException();

        public override void GenerateProperty(Property property, ClassSource classSource)
        {
        }

        public override void GenerateAttachedProperty(AttachedProperty attachedProperty, ClassSource mainClassSource, ClassSource attachedClassSource)
        {
        }
    }
#endif
}
