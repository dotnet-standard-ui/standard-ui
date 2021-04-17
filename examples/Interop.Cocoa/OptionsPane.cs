using System;
using System.Diagnostics;
using AppKit;
using Foundation;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.State;
using Native = Microsoft.StandardUI.Cocoa.Native;

namespace Interop.Cocoa
{
    // Use record to avoid writing constructor boilerplate.
    // Use State<T> to enable internal modification (records can't be modified in place).
    public record OptionsData(
        string[] Configurations,
        State<int> SelectedConfiguration,
        string[] Platforms,
        State<int> SelectedPlatform,
        State<bool> GenerateOverflowChecks,
        State<bool> EnableOptimizations,
        State<bool> GenerateXmlDoc,
        State<string> XmlDocPath,
        State<string> Symbols,
        string[] PlatformTargets,
        State<int> SelectedPlatformTarget);

    public record OptionsPane(OptionsData Data, Action Ok, Action Cancel) : RControl
    {
        // README: Close/reopen solution after building the first time. Workaround until VSMac has source generator support.
        // Testing layout here. A real implementation would have to bind to/update a real view model.
        public override Element Build(Context context) =>
            new Column(
                new Row(
                    VerticalAlignment.Center,
                    TextBlock("Configuration:"),
                    Combobox(Data.Configurations, Data.SelectedConfiguration),
                    TextBlock("Platform:"),
                    Combobox(Data.Platforms, Data.SelectedPlatform)
                    ),
                TextBlock("General Options"),
                new Column(
                    Checkbox("Generate overflow checks", Data.GenerateOverflowChecks),
                    Checkbox("Enable optimizations", Data.EnableOptimizations),

                    // By Binding to GenerateXmlDoc we can update Enabled if the checkbox is checked.
                    Data.GenerateXmlDoc.Bind(() =>
                        new Row(
                            VerticalAlignment.Center,
                            Checkbox("Generate xml documentation:", Data.GenerateXmlDoc),
                            TextEdit(Data.XmlDocPath)
                                .Enabled(Data.GenerateXmlDoc)
                                .Width(float.PositiveInfinity)
                                .Expand(),
                            new Native.NSButton()
                                .Title("Browse...")
                                .Enabled(Data.GenerateXmlDoc)
                                .BezelStyle(NSBezelStyle.Rounded)
                            )
                        ),
                    new Row(
                        VerticalAlignment.Center,
                        TextBlock("Define Symbols:"),
                        TextEdit(Data.Symbols)
                            .Width(float.PositiveInfinity)
                        ),
                    new Row(
                        VerticalAlignment.Center,
                        TextBlock("Platform targets:"),
                        Combobox(Data.PlatformTargets, Data.SelectedPlatformTarget)
                        ).Margin(10),
                    new Row(
                        new Native.NSButton()
                            .Title("Cancel")
                            .BezelStyle(NSBezelStyle.Rounded)
                            .Activated(Cancel),
                        new Native.NSButton()
                            .Title("Ok")
                            .BezelStyle(NSBezelStyle.Rounded)
                            .KeyEquivalent("\r")
                            .Activated(Ok)
                        ).Right()
                )
            );

        

        static Native.NSTextField TextBlock(string text) =>
            new Native.NSTextField()
                .StringValue(text)
                .BackgroundColor(NSColor.Clear)
                .Bezeled(false)
                .Editable(false);

        static Native.NSPopUpButton Combobox(string[] options, State<int> selectedIndex) =>
            new Native.NSPopUpButton()
                .Items(options)
                .IndexOfSelectedItem(selectedIndex)
                .Activated((sender, args) => selectedIndex.Value = (int)((NSPopUpButton)sender).IndexOfSelectedItem);

        // We assume we're the only one modifing the underlying state. If that isn't the case,
        // value.Bind(() => ...) can be used to respond to outside influence.
        static Native.NSButton Checkbox(string title, State<bool> value) =>
            new Native.NSButton()
                .Title(title)
                .State(value ? NSCellStateValue.On : NSCellStateValue.Off)
                .Activated(() => value.Value = !value.Value)
                .ButtonType(NSButtonType.Switch);

        static Native.NSTextField TextEdit(State<string> value) =>
            new Native.NSTextField()
                .StringValue(value)
                .Changed((sender, args) =>
                value.Value = ((NSTextField)((NSNotification)sender).Object).StringValue);
    }
}
