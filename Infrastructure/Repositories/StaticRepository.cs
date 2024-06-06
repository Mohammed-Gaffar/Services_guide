using Core.Enums;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StaticRepository
    {
        private readonly DbConn _StaticContext;

        public StaticRepository()
        {
            _StaticContext = new DbConn();
        }

        public string GetUserRole(string Username)
        {

            var  roleName = _StaticContext.users.FirstOrDefault(m => m.UserName == Username).Role;

            if (roleName != null)
            {
                return Roles.Admin.ToString();
            }
            else
            {
                return null;
            }

        }

    }
}
