﻿using System.Diagnostics;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayApp.Extentions;
using PlayApp.Models;

namespace PlayApp.Controllers;

[Authorize]
public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;

    private readonly IServices _services;
    private readonly IUser _user;

    public HomeController(ILogger<HomeController> logger,IServices services, IUser user)
    {
        _logger = logger;
        _services = services;
        _user = user;
    }

    public IActionResult Index()
    {
        IEnumerable<Service> services;
        if (User.IsInRole("Admin")||User.IsInRole("Super_Admin"))
        {
            services = _services.GetAll();
        }
        else
        {
            services = _services.GetAllUser();
        }

        return View(services);
    }

    [Authorize(Roles = "Super_admin,Admin")]
    public IActionResult create()
    {
        return View();
    }

    [Authorize(Roles = "Super_admin,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Service service)
    {
        User createduser;
        if (User.Identity.Name != null)
        {
            createduser = await _user.GetByName(User.Identity.Name);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }

        if (ModelState.IsValid)
        {
            service.Create_At = DateTime.Now;
            service.Created_by = createduser.ID;
            BaseResponse res = await _services.Create(service);
            if (res.IsSuccess == true)
            {
                BasicNotification("تم اضافة البيانات بنجاح", NotificationType.Success);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                BasicNotification(res.Message, NotificationType.Error);
                return RedirectToAction(nameof(create), service);
            }
        }
        else
        {
            return View(service);
        }

    }

    [Authorize(Roles = "Super_admin,Admin")]
    public IActionResult Active(int Id)
    {
        _services.Activate(Id);

        BasicNotification("تم تفعيل الخدمة ", NotificationType.Success);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Super_admin,Admin")]
    public IActionResult DeActive(int Id)
    {
        _services.DeActivate(Id);

        BasicNotification("تم الغاء تفعيل الخدمة ", NotificationType.Warning);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Super_admin,Admin")]
    public async Task<IActionResult>  Edit(int Id) {
        Service service = await _services.GetById(Id);
        return View(service);
    }

    [Authorize(Roles = "Super_admin,Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(Service service)
    {
        User createduser;
        if (User.Identity.Name != null)
        {
            createduser = await _user.GetByName(User.Identity.Name);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }

        if (ModelState.IsValid)
        {
            service.Update_At = DateTime.Now;
            service.Updated_by = createduser.ID;
            BaseResponse res = await _services.Update(service);
            if (res.IsSuccess == true)
            {
                BasicNotification("تم اضافة البيانات بنجاح", NotificationType.Success);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                BasicNotification(res.Message, NotificationType.Error);
                return RedirectToAction(nameof(create), service);
            }
        }
        return View(service);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
