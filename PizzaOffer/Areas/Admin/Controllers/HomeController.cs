using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOffer.Areas.Web.Models.Admin;
using PizzaOffer.Common;
using PizzaOffer.Services;

namespace PizzaOffer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[Area]/[controller]/[action]")]
    [Authorize(Policy = CustomRoles.Admin)]
    public class HomeController : Controller
    {
        private readonly IUsersService _usersService;

        public HomeController(IUsersService usersService)
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