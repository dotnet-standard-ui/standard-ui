// This file is generated from IPropertyPath.cs. Update the source file to change its contents.

using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms
{
    public class PropertyPath : StandardUIBindableObject, IPropertyPath
    {
        public static readonly BindableProperty PathProperty = PropertyUtils.Register(nameof(Path), typeof(string), typeof(PropertyPath), "");
        
        public string Path => (string) GetValue(PathProperty);
    }
}
