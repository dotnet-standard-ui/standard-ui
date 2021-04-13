using System;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    public class TransformGroup : Transform
    {
        public IReadOnlyList<Transform> Children { get; init; } = Array.Empty<Transform>();
    }
}
