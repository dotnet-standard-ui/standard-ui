using System;
using System.Runtime.CompilerServices;
using Microsoft.StandardUI.Cocoa.Native.Internal;

namespace Microsoft.StandardUI.Cocoa.Native
{
    public abstract partial class NSPopUpButtonBase<TSubclass, TView>
    {
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
