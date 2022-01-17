using System.Collections.Generic;

namespace Microcharts
    /// <summary>
{
    /// Base class of simple chart
    /// </summary>
    public abstract class SimpleChart : Chart
    {
        public SimpleChart(IChart control) : base(control)
        {
        }

#if false
        /// <summary>
        /// Gets or Sets Entries
        /// </summary>
        /// <value>IEnumerable of <seealso cref="T:Microcharts.ChartEntry"/></value>
        public IEnumerable<ChartEntry> Entries
        {
            get => entries;
            set => UpdateEntries(value);
        }
#endif
    }
}
