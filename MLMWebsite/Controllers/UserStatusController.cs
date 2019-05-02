using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLMWebsite.Data;
using MLMWebsite.Models;

namespace MLMWebsite.Controllers
{
    public class UserStatusController : Controller
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> userManager;
        public readonly RoleManager<IdentityRole> roleManager;
        public UserStatusController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager
                                    , RoleManager<IdentityRole> _roleManager)
        {
            _context = context;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public async Task<IActionResult> UpdateData(int id)
        {
            // id approver id proof id proof 1 application user
            // 1. Check Proof count is equal to 10
            // 2. Upload 10 proof approved by 10 approvers

            UserStatus data = new UserStatus();
            var userid = User.getUserId();
            data.ApproverID = userid;
            data.ProofID = id;

            var proofuser = _context.Proof.Include(s => s.ApplicationUser)
                                        .Where(p => p.Id == id);
            foreach (var item in proofuser)
            {

                var dsta = item.ApplicationMemberId;
                var userapproving = await userManager.FindByIdAsync(dsta);
                if (userapproving.ApprovalCount <= 9)
                {
                    userapproving.ApprovalCount += 1;
                    var result = await userManager.UpdateAsync(userapproving);

                    ModelState.AddModelError("", "User not updated, something went wrong.");
                }
                else
                {
                    // Update Role
                    var updaterole = await userManager.AddToRoleAsync(userapproving, "InitAdmin");
                }

            }
            _context.UserStatus.Add(data);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}