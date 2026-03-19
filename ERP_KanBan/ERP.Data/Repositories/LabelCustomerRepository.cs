using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class LabelCustomerRepository : Bases.BaseRepository<LabelCustomer>
    {
        public LabelCustomerRepository(DbContext db) : base(db)
        {
        }
    }
}