using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class MpsStyleRepository : Bases.BaseRepository<MpsStyle>
    {
        public MpsStyleRepository(DbContext db) : base(db)
        {
        }
    }
}
