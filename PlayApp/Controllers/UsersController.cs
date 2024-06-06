using Core.Entities;
using Core.Interfaces;
using Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using PlayApp.Extentions;
namespace PlayApp.Controllers
{
    public class UsersController : BaseController
    {
        private readonly DbConn _db;

        private readonly IUser _IUser;

        public UsersController(IUser user,DbConn dbConn)
        {
            _IUser = user;
            _db = dbConn;
        }

        public IActionResult Index()
        {
            var Users = _IUser.GetAll();

            return View(Users);
        }

        public IActionResult create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {

            if (ModelState.IsValid)
            {
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


        public async Task<IActionResult> Edit(int Id)
        {

            var user  = await _IUser.GetById(Id);

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (ModelState.IsValid)
            {
                user.Updated_by = Convert.ToInt32(User.Identity.Name);
                user.Update_At = DateTime.Now;

                BaseResponse res = await _IUser.Update(user);

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
                return View(user);
            }
        }


    }
}
