using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.Interop;
using Microsoft.StandardUI.Tree;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Colors = Microsoft.StandardUI.Drawing.Colors;
using DpiScale = Microsoft.StandardUI.Drawing.DpiScale;
using WpfDpiScale = System.Windows.DpiScale;
using WpfRect = System.Windows.Rect;
using WpfSize = System.Windows.Size;

namespace Microsoft.StandardUI.Wpf
{
    public class Host : global::System.Windows.Controls.Control
    {
        StateManager stateManager;
        Root root;
        Func<Element> rootElement = () => new Dummy();
        bool initialized;
        RootLayer rootLayer;
        DpiScale dpi = new();
        FontManager fontManager = new FontManager(SKFontManager.Default);

        public Host()
        {
            var source = PresentationSource.FromVisual(this);
            var source2 = PresentationSource.CurrentSources.Cast<PresentationSource>().FirstOrDefault();
            stateManager = new(action => Dispatcher.BeginInvoke(DispatcherPriority.DataBind, action));
            rootLayer = new();
            AddVisualChild(rootLayer);
            root = new(CreateContext(), rootElement);
            root.RootNodeChanged += Root_RootNodeChanged;
            rootLayer.RootNode = root.RootNode;
            Focusable = false;
        }

        public Host(Func<Element> rootElement) : this() => RootElement = rootElement;

        void Root_RootNodeChanged(object? sender, EventArgs e) =>
            rootLayer.RootNode = root.RootNode;

        public Func<Element> RootElement
        {
            get => rootElement;
            set
            {
                rootElement = value;
                if (initialized)
                    root.RootElement = value;
            }
        }

        public void Refresh() => root.Refresh();

        protected override WpfSize ArrangeOverride(WpfSize finalSize)
        {
            MaybeInitialize();
            rootLayer.Arrange(new(finalSize));
            return finalSize;
        }

        protected override WpfSize MeasureOverride(WpfSize availableSize)
        {
            MaybeInitialize();
            rootLayer.Measure(availableSize);
            return rootLayer.DesiredSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0)
                return rootLayer;
            else
                throw new IndexOutOfRangeException();
        }

        protected override int VisualChildrenCount => 1;

        protected override void OnDpiChanged(WpfDpiScale oldDpi, WpfDpiScale newDpi)
        {
            base.OnDpiChanged(oldDpi, newDpi);
            dpi = newDpi.Into();
            root.Context = CreateContext();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            var property = e.Property;
            if (property == FontSizeProperty || property == FontFamilyProperty ||
                property == FontStyleProperty || property == FontWeightProperty)
                root.Context = CreateContext();
        }

        Context CreateContext()
        {
            var familyName = FontFamily.Source;
            if (familyName == "Segoe UI")
                familyName = "Segoe UI Emoji";
            var fontSlant = FontSlant.Normal;
            if (FontStyle == FontStyles.Italic)
                fontSlant = FontSlant.Italic;
            else if (FontStyle == FontStyles.Oblique)
                fontSlant = FontSlant.Oblique;

            var typeface = fontManager.CreateTypeface(familyName, fontSlant, FontWeight.ToOpenTypeWeight());
            return new Context(stateManager, rootLayer, new Dictionary<Type, object>
            {
                { typeof(DpiScale), dpi },
                { typeof(TextTheme), new TextTheme(typeface, (float)FontSize, Colors.Black) },
                { typeof(IFontManager), fontManager }
            });
        }

        void MaybeInitialize()
        {
            if (initialized)
                return;

            var source = PresentationSource.FromVisual(this);
            if (source != null)
            {
                var dpix = source.CompositionTarget.TransformToDevice.M11;
                var dpiy = source.CompositionTarget.TransformToDevice.M22;
                var currentDpi = new DpiScale((float)dpix, (float)dpiy);
                if (currentDpi != dpi)
                {
                    dpi = currentDpi;
                    root.Context = CreateContext();
                }
            }

            initialized = true;
            root.RootElement = rootElement;
        }
    }
}
