using Microsoft.StandardUI;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.State;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.StandardUI.Elements.Factories;
using WpfButton = Microsoft.StandardUI.Wpf.System.Windows.Controls.Button;
using WpfComboBox = Microsoft.StandardUI.Wpf.System.Windows.Controls.ComboBox;

WpfButton WpfButton() => new();
WpfComboBox WpfComboBox() => new();

/// <summary>
/// Simple "immutable" state management
/// 
/// Prior Art: Vanilla React
/// Pros: Simplest
/// Cons: Does more work on state change than Wpf
/// 
/// For effienct render/animations layout+rendering the framework needs to optimize layout/rendering/state
/// transfer anyways. On complex state updates StandardUI controls can skip any "diff" logic and just invalidate
/// layout+render. For native controls this may be less efficient.
/// 
/// Note: Common misconception is that the "entire" application will be invalidated every time state changes.
///       This is incorrect in practice. Each control (or component) should have its own state. Might
///       be difficult to message this/convince existing Wpf developers this approach is fine.
/// </summary>
Element StateExample() =>
    State.Inject<int>((state, setState) =>
        HStack(
            WpfButton()
                .Content($"Hello world {state}")
                .OnClick(_ => setState(state + 1))
                .Margin(10),
            Button(
                content: $"Hello World 🐱‍🐉 {state}",
                click: () => setState(state + 1)
            ).Margin(10)
        ));

/// <summary>
/// Same as simple "State", but don't track changes at the injection point.
/// 
/// Prior Art: ???
/// Pros: Minimal updates/minimal overhead (similar to Wpf bindings)
/// Cons: If you accidentally access state outside of "Bind" you won't react anymore.
/// 
/// Could be valuable for auto generated code. A markup compiler can figure out where
/// state is accessed and insert the nessisary bindings. Alternatively if users are
/// experiencing perf issues due to regular State behavior, they can switch to UnsafeState
/// and optimize that section of code by hand.
/// </summary>
Element UnsafeStateExample() =>
    State.UnsafeInject<int>(state =>
        HStack(
            WpfButton()
                .Content(state.Bind(() => $"Hello world {state}"))
                .OnClick(_ => state.Value += 1)
                .Margin(10),
            Button(content: state.Bind(() => $"Hello World 🐱‍🐉 {state}"), click: () => state.Value += 1).Margin(10)
        ));

/// <summary>
/// Eliminate accidental usage of UnsafeState by not providing access to the underlying Value.
/// Calls to Bind or Set are needed to access the state's value.
/// 
/// Prior Art: ??? (It probably exists somewhere)
/// Pros: Just as efficient as UnsafeState but safe!
/// Cons: API is slightly less ergonimic than State or UnsafeState
/// 
/// This is an attempt at making "UnsafeState" safe. You can't access the state's value
/// unless you explicitly bind to it. It's possible to leak the value by abusing Set, but that sort
/// of abuse should be easy to catch in CR or with lints.
/// 
/// Catch: Since everything implements ToString you can accidentally use string interpolation
///        on the "state". There's no good way of protecting against this at compile time.
///        This could get confusing for new users.
/// </summary>
Element ExplicitStateExample() =>
    ExplicitState.Inject<int>(state =>
        HStack(
            WpfButton()
                .Content(state.Bind(value => $"Hello world {value}"))
                .OnClick(_ => state.Set(c => c + 1))
                .Margin(10),
            Button(
                content: state.Bind(value => $"Hello World 🐱‍🐉 {value}"),
                click: () => state.Set(c => c + 1))
                .Margin(10)
        ));

/// <summary>
/// State can be access anywhere
/// 
/// Prior Art: Mobx, ☄
/// Pros: Leads to very clean example code with minimal updates.
/// Cons: Major additional complexity in the framework/user control external apis. Lambdas everywhere.
/// 
/// Works introducing "scopes" where change is managed/regenerated. These scopes are inserted when users
/// pass a lambda instead of an explicit value. Control implementors need to accept both regular values
/// and lambdas. They also need to wrap the lambdas.
/// 
/// Framework complexity: Biggest issue is we can't have implicit conversion from lambdas
///                       to user defined types. This means you need to have lambda/regular
///                       variants for everything that can be "bound" to. Additionally, there
///                       isn't a discriminating union type. This means we can't mix regular
///                       and lambda variants in a collection. C# 10 request??
/// Threading: C#'s threading model means you need to under (or maybe over?) estimate what needs to be bound
///            to in each scope. This is only and issue if apps are switching between threads while
///            generating multiple UI trees.
/// </summary>
Element ImplicitStateExample() =>
    ImplicitState.Inject<int>(state =>
        HStack(
            WpfButton()
                .Content("TODO")
                .OnClick(_ => state.Value += 1)
                .Margin(10),
            Button(
                content: new ScopeElement(() => $"Hello World 🐱‍🐉 {state}"),
                click: () => state.Value += 1)
                .Margin(10)
        ));

App.Run(() => {
    List<(string, Element)> examples = new()
    {
        (nameof(StateExample), StateExample()),
        (nameof(UnsafeStateExample), UnsafeStateExample()),
        (nameof(ExplicitStateExample), ExplicitStateExample()),
        (nameof(ImplicitStateExample), ImplicitStateExample()),
    };
    var exampleNames = examples.Select(ex => ex.Item1).ToList();

    return State.Inject<int>((state, setState) => VStack(
            HorizontalAlignment.Center,
            WpfComboBox()
                .ItemsSource(exampleNames)
                .SelectedIndex(state)
                .OnSelectionChanged(args =>
                {
                    var selection = (string)args.AddedItems[0]!;
                    var index = examples.FindIndex(v => v.Item1 == selection);
                    setState(index);
                })
                .Margin(2),
            examples[state].Item2
        ));
});