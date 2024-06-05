using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUser
    {
        public IEnumerable<User> GetAll();
        public Task<BaseResponse> GetById(int id);
        public Task<BaseResponse> Create(User User);
        public Task<BaseResponse> Update(User User);
        public Task<BaseResponse> DeleteById(int id);
    }
}
