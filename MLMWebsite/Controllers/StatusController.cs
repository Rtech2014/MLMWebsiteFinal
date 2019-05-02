using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLMWebsite.Data;
using MLMWebsite.Models;

namespace MLMWebsite.Controllers
{
    public class StatusController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public StatusController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var userApprove = await _userManager.GetUserAsync(User);

            var approvaluser = _userManager.Users.Where(s => s.ApprovalCount < 10);
            return View(await approvaluser.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Approve(string id)
        {
            var userdata = await _userManager.FindByIdAsync(id);
            if (userdata.ApprovalCount < 10 ) {
                userdata.ApprovalCount += 1;
                var result = await _userManager.UpdateAsync(userdata);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }
            else if(userdata.ApprovalCount == 10)
            {
                var userrole = await _userManager.AddToRoleAsync(userdata, "InitAdmin");

                return RedirectToAction("Index", "Home");

            }
            else
            {
                return View("Errorpage");
            }
            return View();
        }
    }
}