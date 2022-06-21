using System;
using Microsoft.Maui.Controls;

namespace Microsoft.StandardUI.Maui
{
    public static class PropertyUtils
    {
        public static BindableProperty Register(string propertyName, Type propertyType, Type ownerType, object? defaultValue) =>
            BindableProperty.Create(propertyName, propertyType, ownerType, defaultValue: defaultValue,
                propertyChanged: OnPropertyChanged);

        public static BindableProperty RegisterAttached(string propertyName, Type propertyType, Type ownerType, object? defaultValue) =>
            BindableProperty.CreateAttached(propertyName, propertyType, ownerType, defaultValue: defaultValue,
                propertyChanged: OnPropertyChanged);

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
