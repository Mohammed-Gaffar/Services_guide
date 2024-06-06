using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IServices
    {
        public IEnumerable<Service> GetAll();
        public Task<Service> GetById(int id);
        public Task<BaseResponse> Create(Service service);
        public Task<BaseResponse> Update(Service service);
        public Task<BaseResponse> DeActivate(int id);
        public Task<BaseResponse> Activate(int id);
        public Task<Service> GetByName(string Name);

    }
}
