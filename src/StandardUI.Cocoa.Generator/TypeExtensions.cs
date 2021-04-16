using System;
using Microsoft.CodeAnalysis;

namespace StandardUI.Cocoa.Generator
{
    public static class TypeExtensions
    {
        public static bool IsSubclassOf(this INamedTypeSymbol me, INamedTypeSymbol super)
        {
            var current = me.BaseType;
            while (current != null)
            {
                if (current.Equals(super, SymbolEqualityComparer.Default))
                    return true;
                current = current.BaseType;
            }

            return false;
        }

        // Only works for private/protected/friend/internal/public for now
        public static string Format(this Accessibility accessibility) => accessibility.ToString().ToLower();
    }
}
