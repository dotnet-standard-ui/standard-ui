using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.XamarinForms.Media;
using System;
using System.Collections;
using System.Linq;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms
{
    public static class PropertyUtils
    {
        public static readonly ColorXamarinForms DefaultColor = ColorXamarinForms.Transparent;
        public static readonly PointXamarinForms DefaultPoint = PointXamarinForms.Default;
        public static readonly SizeXamarinForms DefaultSize = SizeXamarinForms.Default;

        public static IEnumerable Empty<T>()
        {
            return Enumerable.Empty<T>();
        }

        public static BindableProperty Register(string propertyName, Type propertyType, Type ownerType, object? defaultValue)
        {
            if (propertyType == typeof(IBrush))
                propertyType = typeof(Microsoft.StandardUI.XamarinForms.Media.Brush);
            else if (propertyType == typeof(ITransform))
                propertyType = typeof(Transform);
            else if (propertyType == typeof(Color))
            {
                propertyType = typeof(ColorXamarinForms);
                defaultValue = ColorXamarinForms.Transparent;
            }

            return BindableProperty.Create(propertyName, propertyType, ownerType, defaultValue: defaultValue,
                propertyChanged: OnPropertyChanged);
        }

        public static BindableProperty RegisterAttached(string propertyName, Type propertyType, Type ownerType, object? defaultValue)
        {
            return BindableProperty.CreateAttached(propertyName, propertyType, ownerType, defaultValue: defaultValue,
                propertyChanged: OnPropertyChanged);
        }

        private static void OnPropertyChanged(BindableObject obj, object oldValue, object newValue)
        {
            if (!(obj is INotifyObjectOrSubobjectChanged parentObj))
                return;

            // The logic below cascades change notifications from subobjects up the object hierarchy, eventually causing the GraphicsCanvas
            // to be invalidated on any change
            if (oldValue is INotifyObjectOrSubobjectChanged oldChildObj)
                oldChildObj.Changed -= parentObj.NotifySinceSubobjectChanged;

            if (newValue is INotifyObjectOrSubobjectChanged newChildObj)
                newChildObj.Changed += parentObj.NotifySinceSubobjectChanged;

            parentObj.NotifySinceObjectChanged();
        }
    }
}
