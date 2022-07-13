using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using System.ComponentModel;
using static Microsoft.StandardUI.StandardUIStatics;

namespace AlohaKit.StandardControls
{
    [StandardControl]
    public interface IToggleSwitch : IStandardControl
    {
        public Color BackgroundColor { get; set; }

        public Color ThumbColor { get; set; }

        [DefaultValue(false)]
        public bool IsOn { get; set; }

        [DefaultValue(true)]
        public bool HasShadow { get; set; }
    }

    public class ToggleSwitchImplementation : StandardControlImplementation<IToggleSwitch>
    {
        public ToggleSwitchImplementation(IToggleSwitch control) : base(control)
        {
            Control.Width = 40;
            Control.Height = 30;
        }

        public override IUIElement? Build() =>
            Canvas()._(
                Rectangle()
                    .Width(Control.Width)
                    .Height(Control.Height)
                    .RadiusX(16)
                    .RadiusY(16)
                    .Fill(SolidColorBrush(Control.BackgroundColor)),

                Ellipse()
                    .CanvasLeft(Control.IsOn ? 35 : 15)
                    .CanvasTop(3)
                    .Width(24)
                    .Height(24)
                    .Fill(SolidColorBrush(Control.ThumbColor))
            );
    }
}
