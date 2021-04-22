[![CI](https://github.com/dotnet-standard-ui/standard-ui/actions/workflows/ci.yml/badge.svg)](https://github.com/dotnet-standard-ui/standard-ui/actions/workflows/ci.yml)

# 🐱‍🐉 .NET Standard UI (aka DinoCat)

## Value Propositions

.NET Standard UI aims to help solve these problems:

**Grow the Microsoft UI control ecosystem** - Writing a single control that can target several UI
frameworks means it's easier to write controls and they can target a bigger set of users. If you're writing say a chart control or a fancy radial gauge, no longer do you need to write separate versions for WPF/Xamarin.Forms/UWP/WinUI/Uno or create your own platform wrappers to share code.
.NET Standard UI allows a single control assembly that works on all supported UI frameworks.
UI framework specific control APIs (e.g. DependencyProperty for control properties on WPF/UWP/WinUI properties and
BindableProperty for Xamarin/MAUI) are generated as usage time, via source generators.
This helps control vendors, community members that build controls, and Microsoft as it builds out first
party controls - cheaper + wider reach should mean more controls in the ecosystem. For Microsoft controls, possibilities include cross platform Fluent UI or controls that interoperate with MS services, like the MS Graph controls [here](https://docs.microsoft.com/en-us/windows/communitytoolkit/graph/controls/peoplepicker). 

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

## Beautiful UI Anywhere

Create high quality UI that is consistent and runs with native performance across a wide variety of devices. Write controls that integrate seamlessly with existing C# UI frameworks, initially targeting WPF and Cocoa.

## Easy to Learn and Use

If you already know C# you can get started writing UI today! Standard UI makes your life easier by reducing the number of new concepts you have to learn. You can take advantage of the next generation of C# tooling to make writing UI highly efficient.

If you're already using an existing C# UI framework (WinForms, Wpf, Xamarin, Uwp, WinUI) that's great too! You can start using StandardUI in your current projects by referencing a single NuGet package. You can even write new controls in StandardUI that reuse your existing user controls and view models. StandardUI works best with MVVM so your existing experience transfers directly into StandardUI.

## Progress

* Basic Wpf integration
  * Projections from Wpf to DinoCat (need better container support)
  * Projections from DinoCat to Wpf (very early)
* Cross platform rendering with SkiaSharp
* RTL layout

## Missing Features

* Animations
* Control library
* Rich text layout (Multiline, mixed styles/fonts, bidi, ect)
* WinForms/Xamarin/Uwp/WinUI integration
* HotReload integration (DinoCat is set up for hot reload. Once the tooling catches up this should be easy...)
* Publish to NuGet
* And lots more... Everything is very early and needs another iteration or so...
