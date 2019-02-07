using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOffer.Areas.Web.Models.Admin;
using PizzaOffer.Common;
using PizzaOffer.Services;

namespace PizzaOffer.Areas.Web.Controllers.Admin
{
    [Area("Web")]
    [Authorize(Policy = CustomRoles.Admin)]
    public class AdminController : Controller
    {
        private readonly IUsersService _usersService;

        public AdminController(IUsersService usersService)
        {
            this._usersService = usersService;
            _usersService.CheckArgumentIsNull(nameof(usersService));
        }

        public async Task<IActionResult> Index()
        {
            var userList = await _usersService.GetAllUsersAsync();
            //Todo: when automapper library added map userList to userViewModel list and return userviewmodel
            return View(userList);
        }
    }
}