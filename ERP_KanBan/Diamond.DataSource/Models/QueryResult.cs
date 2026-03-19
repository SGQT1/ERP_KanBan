using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Diamond.DataSource.Models
{
    public class QueryResult
    {
        public int Total { get; set; }
        public IEnumerable Data { get; set; }
    }
}