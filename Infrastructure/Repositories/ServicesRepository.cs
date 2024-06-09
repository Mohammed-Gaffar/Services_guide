using Core.Entities;
using Core.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ServicesRepository : IServices
    {
        private readonly DbConn _conn;

        public ServicesRepository(DbConn dbConn)
        {
            _conn = dbConn;
        }
        public async Task<BaseResponse> Activate(int id)
        {
            try
            {
                var DbService = _conn.services.Find(id);
                DbService.IsActive = true;
                await _conn.SaveChangesAsync();

                return new BaseResponse { IsSuccess = true};
            }
            catch (Exception ex)
            {

                return new BaseResponse { IsSuccess = false };
            }
            
        }

        public async Task<BaseResponse> Create(Service service)
        {
            await _conn.services.AddAsync(service);
            await _conn.SaveChangesAsync();
            return new BaseResponse { IsSuccess = true };
        }

        public async Task<BaseResponse> DeActivate(int id)
        {
            try
            {
                var DbService = _conn.services.Find(id);
                DbService.IsActive = false;
                await _conn.SaveChangesAsync();

                return new BaseResponse { IsSuccess = true };
            }
            catch (Exception ex)
            {

                return new BaseResponse { IsSuccess = false };
            }
        }

        public IEnumerable<Service> GetAll()
        {
           return _conn.services;
        }

        public IEnumerable<Service> GetAllUser()
        {
            return _conn.services.Where(m => m.IsActive == true);
        }

        public async Task<Service> GetById(int id)
        {
            Service service = await _conn.services.FindAsync(id);
            if (service != null)
                return service;

            return null;

        }
        public async Task<Service> GetByName(string Name)
        {
            Service service = await _conn.services.FirstOrDefaultAsync(m => m.Name == Name);

            return service;
        }

        public async Task<BaseResponse> Update(Service service)
        {
            var DbService = await _conn.services.FindAsync(service.ID);
            if (DbService != null)
            {
                service.Create_At = DbService.Create_At;
                service.Created_by = DbService.Created_by;

                _conn.Entry(DbService).CurrentValues.SetValues(service);
                await _conn.SaveChangesAsync(true);
                return new BaseResponse { IsSuccess = true, Message = "تم تحديث بيانات الخدمة " };
            }
            else
            {
                return new BaseResponse() { IsSuccess = false, Message = "الرجاء  التحقق من العملية " };
            }
        }
    }
}
