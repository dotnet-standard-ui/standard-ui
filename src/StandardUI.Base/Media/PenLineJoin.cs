namespace Microsoft.StandardUI.Media
{
    /// <summary>
    /// Describes the shape that joins two lines or segments.
    /// </summary>
    public enum PenLineJoin
    {
        /// <summary>
        /// Line joins use regular angular vertices.
        /// </summary>
        Miter = 0,

        /// <summary>
        /// Line joins use beveled vertices.
        /// </summary>
        Bevel = 1,

        /// <summary>
        /// Line joins use rounded vertices.
        /// </summary>
        Round = 2
    }
}
