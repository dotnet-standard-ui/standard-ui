using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using System.ComponentModel;
using static Microsoft.StandardUI.StandardUIStatics;

namespace SimpleControls
{
    public interface IRadialGauge : IStandardControl
    {
        [DefaultValue(null)]
        IBrush? Fill { get; set; }
    }

    public class RadialGauge<T> : StandardControlImplementation<T> where T : IRadialGauge
    {
        public RadialGauge(T control) : base(control)
            { }

        public override IUIElement? Build()
        {
            var blueBrush = SolidColorBrush().Color(Colors.Blue);
            return Rectangle() .Width(50) .Height(50) .Stroke(blueBrush) .Fill(Control.Fill);
        }
    }
}
