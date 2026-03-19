using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories
{
    public class OrdersPLPhotoRepository : Bases.BaseRepository<OrdersPLPhoto>
    {
        public OrdersPLPhotoRepository(DbContext db) : base(db) { }
    }
}