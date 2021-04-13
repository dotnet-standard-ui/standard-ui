[![CI](https://github.com/dotnet-standard-ui/standard-ui/actions/workflows/ci.yml/badge.svg)](https://github.com/dotnet-standard-ui/standard-ui/actions/workflows/ci.yml)

# 🐱‍🐉 .NET Standard UI (aka DinoCat)

A proof of concept C# UI library. Goals are aspirational. See [Progress](README.md#Progress) for current progress.

## Beautiful UI Anywhere

Create high quality UI that is consistent and runs with native performance across a wide variety of devices. Write controls that integrate seamlessly with existing C# UI frameworks, initially targeting WPF and Cocoa.

## Easy to Learn and Use

If you already know C# you can get started writing UI today! DinoCat makes your life easier by reducing the number of new concepts you have to learn. You can take advantage of the next generation of C# tooling to make writing UI highly efficient.

If you're already using an existing C# UI framework (WinForms, Wpf, Xamarin, Uwp, WinUI) that's great too! You can start using DinoCat in your current projects by referencing a single NuGet package. You can even write new controls in DinoCat that reuse your existing user controls and view models. DinoCat works best with MVVM so your existing experience transfers directly into DinoCat.

## Progress

* Basic Wpf integration
  * Projections from Wpf to DinoCat (need better container support)
  * Projections from DinoCat to Wpf (very early)
* Cross platform rendering with SkiaSharp
* RTL layout

## Missing Features

* Animations
* Control library
* Rich text layout
* WinForms/Xamarin/Uwp/WinUI integration
* HotReload integration (DinoCat is set up for hot reload. Once the tooling catches up this should be easy...)
* Publish to NuGet
* And lots more... Everything is very early and needs another iteration or so...
