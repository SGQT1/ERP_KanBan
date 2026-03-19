using System.Collections.Generic;

namespace Diamond.DataSource.Models
{
    public class QueryRequest
    {
        public IEnumerable<SortDescriptor> Sort { get; set; }
        public IEnumerable<FieldDescriptor> Select { get; set; }
        public IEnumerable<FieldDescriptor> GroupBy { get; set; }
        public IFilterDescriptor Filter { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public bool? Count { get; set; }
        public string[] Extention { get; set;}
    }
}