// This file is generated from ITransformGroup.cs. Update the source file to change its contents.

using System.Collections.Generic;
using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class TransformGroup : Transform, ITransformGroup
    {
        public static readonly BindableProperty ChildrenProperty = PropertyUtils.Create(nameof(Children), typeof(IEnumerable<ITransform>), typeof(TransformGroup), null);
        
        public IEnumerable<ITransform> Children => (IEnumerable<ITransform>) GetValue(ChildrenProperty);
    }
}
