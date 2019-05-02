using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.ViewComponents
{
    public class SuperAdminViewComponent:ViewComponent

    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuperAdminViewComponent(ApplicationDbContext context , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var super = _userManager.Users.Where(s=>s.Email.Contains("superadmin@gmail.com")).FirstOrDefault();
            return View(super);
        }
    }
}
