using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class UserService : EntityService<users>
    {
        protected new UserRepository Repository { get { return base.Repository as UserRepository; } }

        public UserService(UserRepository repository) : base(repository)
        {
        }
    }
}