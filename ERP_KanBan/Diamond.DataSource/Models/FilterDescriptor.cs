namespace Diamond.DataSource.Models
{
    public class FilterDescriptor : IFilterDescriptor
    {
        public string Field { get; set; }
        public FilterOperator Operator { get; set; }
        public object Value { get; set; }
    }
}