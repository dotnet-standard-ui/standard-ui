using Microsoft.StandardUI;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.State;
using static Microsoft.StandardUI.Elements.Factories;

App.Run(() =>
    State.Inject<int>((count, setCount) => HStack(
        VerticalAlignment.Center,
        Text("Hello World 🐱‍🐉")
            .Margin(2),
        Button(
            content: $"Clicked {count} time(s)!!",
            click: () => setCount(count + 1)))));