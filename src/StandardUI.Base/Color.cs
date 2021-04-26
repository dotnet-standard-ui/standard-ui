using Microsoft.StandardUI.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.StandardUI
{
    public struct Color : IEquatable<Color>
    {
        private uint color;

        public static implicit operator Paint(Color c) => new Paint(fill: c);
        public static implicit operator Pen(Color c) => new Pen(c);
        public static implicit operator Brush(Color c) => new SolidColorBrush(c);

        public Color(uint value) => color = value;
        public Color(byte red, byte green, byte blue, byte alpha = 255) =>
            color = (uint) ((alpha << 24) | (red << 16) | (green << 8) | blue);

        public byte Alpha => (byte)((color >> 24) & 0xFF);
        public byte Red => (byte)((color >> 16) & 0xFF);
        public byte Green => (byte)((color >> 8) & 0xFF);
        public byte Blue => (byte)(color & 0xFF);

        public static implicit operator Color(uint color) => new Color(color);
        public static explicit operator uint(Color color) => color.color;

        public static bool operator ==(Color left, Color right) => left.color == right.color;
        public static bool operator !=(Color left, Color right) => left.color != right.color;

        public bool Equals(Color other) => color == other.color;

        public override bool Equals(object? obj) =>
            obj is Color c && color == c.color;

        public override int GetHashCode() => color.GetHashCode();
    }
}
