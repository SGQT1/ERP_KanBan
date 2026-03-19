using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsLiveItemRepository : Bases.BaseRepository<MpsLiveItem>
    {
        public MpsLiveItemRepository(DbContext db) : base(db)
        {
        }
    }
}
