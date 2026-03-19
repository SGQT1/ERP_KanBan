using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Data.Repositories
{
    public class ReceivedLogRepository : BaseRepository<ReceivedLog>
    {
        public ReceivedLogRepository(DbContext db) : base(db)
        {
        }
    }
}
