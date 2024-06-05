using Core.Entities;
using Core.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Formats.Asn1;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUser
    {
        private readonly DbConn _db;
        public UserRepository(DbConn dbConn)
        {
            _db = dbConn;
        }
        public async Task<BaseResponse> Create(User User)
        {
            var check_user =  await _db.users.FirstOrDefaultAsync(m => m.UserName == User.UserName) ; 

            if ( check_user == null )
            {
                var res = _db.users.Add(User);
                await _db.SaveChangesAsync();
                return new BaseResponse { IsSuccess = true , Message = "تم اضافة المستخدم بنجاح" };
            }
            else
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "لم تتم الاضافة المستخدم موجود بالفعل",
                };
            }
        }

        public Task<BaseResponse> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
         
            return _db.users.ToList(); 
        }

        public Task<BaseResponse> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> Update(User User)
        {
            throw new NotImplementedException();
        }
    }
}
