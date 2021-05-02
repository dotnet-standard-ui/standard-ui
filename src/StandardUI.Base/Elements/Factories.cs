using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Elements.Events;
using System;

namespace Microsoft.StandardUI.Elements
{
    public static class Factories
    {
        public static TextBlock Text(string text, Brush? foreground = null, float? fontSize = null, ITypeface? typeface = null) =>
            new(text, foreground, fontSize, typeface);
        public static TextBlock TextBlock(string text, Brush? foreground = null, float? fontSize = null, ITypeface? typeface = null) =>
            Text(text, foreground, fontSize, typeface);
        public static Button Button(Element content, Action click) =>
            new(content, click);
        public static HStack HStack(VerticalAlignment alignment, params Expand[] children) => new(alignment, children);
        public static HStack HStack(params Expand[] children) => new(children);
        public static VStack VStack(HorizontalAlignment alignment, params Expand[] children) => new(alignment, children);
        public static VStack VStack(params Expand[] children) => new(children);
        public static ZStack ZStack(params Expand[] children) => new(children);
        public static Input Input(
            ControlType controlType,
            Element child,
            Action? tap = null,
            Action<bool>? gotFocus = null,
            Action? lostFocus = null,
            Action<KeyEvent>? keyDown = null,
            Action<KeyEvent>? keyUp = null,
            Action? mouseEnter = null,
            Action? mouseExit = null,
            Action? hover = null,
            Action? invoke = null,
            string? automationClassName = null,
            string? description = null) => new(controlType, child, tap, gotFocus, lostFocus, keyDown, keyUp, mouseEnter, mouseExit, hover, invoke, automationClassName, description);
        public static Rectangle Rectangle(Paint paint, float? width = null, float? height = null) => new(paint, width, height);
        public static Dummy Dummy() => new();
    }
}
