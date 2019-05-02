using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.ViewComponents
{
    public class ProfilePhotoViewComponent : ViewComponent
    {
       
        private readonly ApplicationDbContext _context;
        public ProfilePhotoViewComponent(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            
            _context = context;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var photo = _context.UserAssets.ToList();
            return View(photo);
        }
    }
}
