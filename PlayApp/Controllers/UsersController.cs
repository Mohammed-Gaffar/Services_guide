using Core.Entities;
using Core.Interfaces;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Transport.NamedPipes;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Update;
using PlayApp.Extentions;
namespace PlayApp.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly DbConn _db;

        private readonly IUser _IUser;

        public UsersController(IUser user,DbConn dbConn)
        {
            _IUser = user;
            _db = dbConn;
        }

        [Authorize(Roles = "Super_admin,Admin")]
        public IActionResult Index()
        {
            var Users = _IUser.GetAll();

            return View(Users);
        }

        [Authorize(Roles ="Super_admin,Admin")]
        public IActionResult create()
        {
            return View();
        }

        [Authorize(Roles = "Super_admin,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            User createduser;
            if (User.Identity.Name != null)
            {
                createduser = await _IUser.GetByName(User.Identity.Name);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                user.Create_At = DateTime.Now;
                user.Created_by = createduser.ID;
                BaseResponse res = await _IUser.Create(user);
                if (res.IsSuccess == true)
                {
                    BasicNotification("تم اضافة المستخدم بنجاح", NotificationType.Success);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    BasicNotification(res.Message, NotificationType.Error);
                    return RedirectToAction(nameof(create),user);
                }
            }
            else
            {
                return View(user);
            }


            return View(nameof(Index));
        }

        [Authorize(Roles = "Super_admin,Admin")]
        public async Task<IActionResult> Edit(int Id)
        {

            var user  = await _IUser.GetById(Id);

            return View(user);
        }

        [Authorize(Roles = "Super_admin,Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            User updatedUser;
            BaseResponse res; 

            if (ModelState.IsValid)
            {
                 
                if (User.Identity.Name != null )
                {
                    updatedUser = await _IUser.GetByName(User.Identity.Name);
                }
                else
                {
                    BasicNotification("يرجى التحقق من تسجيل الدخول", NotificationType.Info);
                    return RedirectToAction(nameof(Index));
                }
                user.Updated_by = updatedUser.ID;
                user.Update_At = DateTime.Now;
                try
                {
                     res = await _IUser.Update(user);
                }
                catch (Exception ex)
                {
                    BasicNotification(ex.Message, NotificationType.Error);
                    return RedirectToAction(nameof(Index));
                }
                

                if (res.IsSuccess == true) {
                    BasicNotification("تم تحديث البانات ", NotificationType.Info);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    BasicNotification(res.Message, NotificationType.Error);
                    return View(user);
                }
            }
            else
            {
                BasicNotification("هنالك خطا الرجاء المحاولة مرة اخرى", NotificationType.Error);
                return View(user);
            }
        }


    }
}
