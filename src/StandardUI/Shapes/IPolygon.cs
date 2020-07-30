﻿using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Shapes
{
    [UIModelObject]
    public interface IPolygon : IShape
    {
        [DefaultValue(FillRule.EvenOdd)]
        FillRule FillRule { get; set; }

        Points Points { get; set; }
    }
}
