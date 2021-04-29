[![CI](https://github.com/dotnet-standard-ui/standard-ui/actions/workflows/ci.yml/badge.svg)](https://github.com/dotnet-standard-ui/standard-ui/actions/workflows/ci.yml)

# 🐱‍🐉 .NET Standard UI (aka DinoCat)

## Overview

.NET Standard UI an experimental project that aims to solve these problems:

**Enable writing portable controls, that work on any .NET UI framework** - .NET Standard UI allows authoring a control (called a Standard Control) that works on all supported UI frameworks, all from a single assembly.
If you're writing say a chart control or a fancy radial gauge, no longer do you need to write separate versions for WPF/Xamarin.Forms/MAUI/UWP/WinUI/Uno/WinForms or create your own platform wrappers to share code.
UI framework specific control APIs (e.g. DependencyProperty for control properties on WPF/UWP/WinUI properties and
BindableProperty for Xamarin/MAUI) are generated as usage time, via source generators.
Writing a single control that can target several UI frameworks means it's easier to write controls and they can target a bigger set of users. 
This helps control vendors, community members that build controls, and Microsoft as it builds out first
party controls - cheaper + wider reach should mean more controls in the ecosystem. For Microsoft controls, possibilities include cross platform Fluent UI or controls that interoperate with MS services, like the MS Graph controls [here](https://docs.microsoft.com/en-us/windows/communitytoolkit/graph/controls/peoplepicker). Even if you don't write any controls yourself, having a bigger set of controls to choose from, no matter what UI framework 
you're using, should make you more productive.

**Reduce .NET UI Fragmentation** - Today there are multiple XAML UI frameworks (WPF, UWP, WinUI, Xamarin.Forms, .NET MAUI, Uno, Avalonia, etc.). All are pretty similar, though slightly different.
For the most part they don't share code. The naming differences are annoying. The binary API differences mean you can't write code (like controls or tools) that work on multiple UI platforms. The lack of a clear vision from MS holds back adoption.
.NET Standard UI helps here by taking a subset of the object model (primarily the subset needed to create controls with drawn UI) and standardizing it. Using a subset makes the problem more tractable - it's not
a single unified XAML object model for everything (not yet in any case), but it's a significant step in that direction.
As the standard is based on WPF/UWP, it means that it isn't a big leap to take an existing WPF/UWP control definition (something like [this](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/controls/button-styles-and-templates?view=netframeworkdesktop-4.8) for instance, constructed out of shape primitives, visual states, and storyboards) and make it a cross platform control.

**Support "build" style UI construction** - Newer UI frameworks, like React and Flutter, allow you to create UI by having a
"build" method, that constructs a UI based on control descriptors (not platform controls). React calls this the virtual DOM.
Whenever state changes, the build method is called (usually), the result is diff'ed with the existing UI, and any changes
are applied. Sometimes diff'ing is combined with change notifications, This style of UI construction has some advantages: It's more powerful than traditional XAML, allowing for more dynamic UI based on state. It also works better with C# code
changes during hot reload, as ALL code (e.g. bindings code) is rerun when regenerating the UI, even if just a single control ends up needing to be updated on the screen. This is similar to [Comet](https://github.com/dotnet/Comet), but
also works for existing UI frameworks, like WPF.

This project is experimental. As such we want your feedback. Is this the right approach? Does it solve real life problems for you? What would you do
differently? Post to [Discussions](https://github.com/dotnet-standard-ui/standard-ui/discussions) or
[Issues](https://github.com/dotnet-standard-ui/standard-ui/issues) to tell us.

## Beautiful UI Anywhere

Create high quality UI that is consistent and runs with native performance across a wide variety of devices. Write controls that integrate seamlessly with existing C# UI frameworks, initially targeting WPF and Cocoa.

## Easy to Learn and Use

If you already know C# you can get started writing UI today! Standard UI makes your life easier by reducing the number of new concepts you have to learn. You can take advantage of the next generation of C# tooling to make writing UI highly efficient.

If you're already using an existing C# UI framework (WinForms, Wpf, Xamarin, Uwp, WinUI) that's great too! You can start using StandardUI in your current projects by referencing a single NuGet package. You can even write new controls in StandardUI that reuse your existing user controls and view models. StandardUI works best with MVVM so your existing experience transfers directly into StandardUI.

## Progress

It's definitely a work in progress. Here's what works currently:

* Basic Wpf and Cocoa integration
  * Projections from Wpf to DinoCat (need better container support)
  * Projections from DinoCat to Wpf (very early)
* Cross platform rendering with SkiaSharp
* RTL layout
