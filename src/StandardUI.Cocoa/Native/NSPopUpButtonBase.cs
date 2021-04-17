using System;
using System.Runtime.CompilerServices;
using Microsoft.StandardUI.Cocoa.Native.Internal;

namespace Microsoft.StandardUI.Cocoa.Native
{
    public abstract partial class NSPopUpButtonBase<TSubclass, TView>
    {
        public static CustomUIProperty<AppKit.NSPopUpButton, int> IndexOfSelectedItemProperty =
            new(0, (view, value) => view.SelectItem(value));

        public TSubclass IndexOfSelectedItem(int index) =>
            Set(IndexOfSelectedItemProperty, index);

        static ConditionalWeakTable<Internal.ViewState, string[]> previousItems = new();
        protected readonly string[] items;

        public TSubclass Items(params string[] items) =>
            With_items(items);

        protected override void Update(TView view, ViewState state)
        {
            base.Update(view, state);
            UpdateItems(view, state);
        }

        void UpdateItems(TView view, ViewState state)
        {
            if (previousItems.TryGetValue(state, out var prev))
            {
                if (prev != items)
                {
                    view.RemoveAllItems();
                    view.AddItems(items);
                }
            }
            else
                view.AddItems(items);
        }
    }
}
