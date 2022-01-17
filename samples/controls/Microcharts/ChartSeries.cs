using System.Collections.Generic;
using Microsoft.StandardUI;

namespace Microcharts
{
    /// <summary>
    /// A series of data entries for chart
    /// </summary>
    public class ChartSeries
    {
        /// <summary>
        /// Gets or sets the name of the serie
        /// </summary>
        /// <value>Name of the serie</value>
        public string Name { get; set; } = "Default";

        /// <summary>
        /// Gets or sets the color of the fill
        /// </summary>
        /// <value>The color of the fill.</value>
        public Color? Color { get; set; } = Colors.Black;

        /// <summary>
        /// Gets or sets the entries value for the serie 
        /// </summary>
        public IEnumerable<ChartEntry> Entries { get; set; }
    }
}
