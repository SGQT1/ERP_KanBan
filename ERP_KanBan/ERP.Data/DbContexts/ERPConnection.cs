using System.Collections.Generic;
using System.Linq;

namespace ERP.Data.DbContexts
{
    public class ERPConnection
    {
        public IQueryable<ERPDataSource> Dbs { get; set; }

        public ERPConnection()
        {
            var items = new List<ERPDataSource>();

            items.Add(new ERPDataSource { LocaleId = 6, Locale = "TDM", Context = "Data Source=192.168.100.19;Initial Catalog=GlobalERP;User ID=sa;Password=sa;MultipleActiveResultSets=true" });
            items.Add(new ERPDataSource { LocaleId = 2, Locale = "CDT", Context = "Data Source=10.192.131.139;Initial Catalog=GlobalERP;User ID=sa;Password=sasa;MultipleActiveResultSets=true" });
            items.Add(new ERPDataSource { LocaleId = 23, Locale = "CYW", Context = "Data Source=192.168.31.248;Initial Catalog=GlobalERP;User ID=sa;Password=sa1234;MultipleActiveResultSets=true" });
            items.Add(new ERPDataSource { LocaleId = 10, Locale = "CSW", Context = "Data Source=192.168.6.248;Initial Catalog=GlobalERP;User ID=sa;Password=sa1234;MultipleActiveResultSets=true" });
            items.Add(new ERPDataSource { LocaleId = 11, Locale = "IDM1", Context = "Data Source=192.168.12.243;Initial Catalog=GlobalERP;User ID=sa;Password=sa1234;MultipleActiveResultSets=true" });
            items.Add(new ERPDataSource { LocaleId = 26, Locale = "IDM2", Context = "Data Source=192.168.14.244;Initial Catalog=GlobalERP;User ID=sa;Password=sa123456;MultipleActiveResultSets=true" });
            items.Add(new ERPDataSource { LocaleId = 7, Locale = "VDM", Context = "Data Source=10.1.1.28;Initial Catalog=GlobalERP;User ID=sa;Password=sa1234;MultipleActiveResultSets=true" });
            items.Add(new ERPDataSource { LocaleId = 18, Locale = "KDM", Context = "Data Source=10.1.100.17;Initial Catalog=GlobalERP;User ID=sa;Password=sa1234;MultipleActiveResultSets=true" });
            items.Add(new ERPDataSource { LocaleId = 25, Locale = "CCW", Context = "Data Source=192.168.6.239;Initial Catalog=GlobalERP;User ID=sa;Password=sa1234;MultipleActiveResultSets=true" });
            items.Add(new ERPDataSource { LocaleId = 30, Locale = "CEY", Context = "Data Source=192.168.21.248;Initial Catalog=GlobalERP;User ID=sa;Password=sa1234;MultipleActiveResultSets=true" });
            items.Add(new ERPDataSource { LocaleId = 99, Locale = "DC", Context = "Data Source=192.168.100.24;Initial Catalog=ERPDataCenter;User ID=sa;Password=sa;MultipleActiveResultSets=true" });
            
            this.Dbs = items.AsQueryable();
        }
    }

    public class ERPDataSource
    {
        public int LocaleId { get; set; }
        public string Locale { get; set; }
        public string Context { get; set; }
    }
}