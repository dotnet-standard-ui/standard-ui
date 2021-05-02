using Microsoft.StandardUI;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.Wpf;
using System;
using static Microsoft.StandardUI.Elements.Factories;
using static Microsoft.StandardUI.Wpf.System.Windows.Controls.Factories;
using static Interop.Wpf.Factories;

namespace Interop.Wpf
{
    [ToWpfType("Interop.Wpf.DinoControlWrapper")]
    class DinoControl : Microsoft.StandardUI.Elements.Control<int>
    {
        public DinoControl(string incomming) => Incomming = incomming;

        public string Incomming { get; }

        public override Element Build(Context context, int state, Action<int> setState) =>
            VStack(
                TextBlock().Text(Incomming),
                MyUserControlWrapper().MyValue(state),
                Button()
                    .Content("Increment 🐱‍🐉")
                    .OnClick(args => setState(state + 1))
                    .Margin(2)
                    .Center(),
                HStack(VerticalAlignment.Baseline,
                    Text("HELLO", fontSize: 32),
                    Button("world", () => { }),
                    Text(" trailing text")),
                MyUserControlWrapper().MyValue(-state));
    }
}
