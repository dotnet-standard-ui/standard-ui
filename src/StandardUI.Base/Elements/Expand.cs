using Microsoft.StandardUI.Tree;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Elements
{
    /// <summary>
    /// Take up remaining space in a <see cref="HStack"/>, <see cref="VStack"/>, or <see cref="ZStack"/>.
    /// </summary>
    /// <remarks><see cref="HStack"/> and <see cref="VStack"/> split their remaing space based on the <see cref="Factor"/>.
    /// For <see cref="ZStack"/> any non-zero factor fills the entire stack.</remarks>
    /// <param name="factor">The amount of space to take up relative to other expanded elements. 0 indicates the element should not be expanded.</param>
    public class Expand
    {
        public Expand(Element child, int factor = 1)
        {
            Child = child;
            Factor = factor;
        }

        public static implicit operator Expand(Element e) => new(e, factor: 0);

        public Element Child { get; }
        public int Factor { get; }
    }

    public static class ExpandHelper
    {
        public static Expand Expand(this Element e, int factor = 1) => new(e, factor);
    }
}
