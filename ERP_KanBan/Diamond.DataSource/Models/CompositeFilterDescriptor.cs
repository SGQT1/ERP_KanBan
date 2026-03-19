using System.Collections.Generic;

namespace Diamond.DataSource.Models
{
    public class CompositeFilterDescriptor : IFilterDescriptor
    {
        public IEnumerable<IFilterDescriptor> Filters { get; set; }
        public FilterCompositionLogicalOperator Logic { get; set; }
    }
}