using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsLiveItemSizeRepository : Bases.BaseRepository<MpsLiveItemSize>
    {
        public MpsLiveItemSizeRepository(DbContext db) : base(db)
        {
        }
    }
}
