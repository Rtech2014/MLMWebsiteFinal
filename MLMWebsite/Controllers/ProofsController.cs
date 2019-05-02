using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MLMWebsite.Data;
using MLMWebsite.Models;

namespace MLMWebsite.Controllers
{
    [Authorize]
    public class ProofsController : Controller
    {
        bool state;
        bool assigned = true;
        private readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> userManager;
        public readonly RoleManager<IdentityRole> roleManager;
        public ProofsController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            _context = context;
            userManager = _userManager;
            roleManager = _roleManager;

        }

        // GET: Proofs
        public async Task<IActionResult> Index()
        {
            var id = User.getUserId();

            var cu_user = await userManager.FindByIdAsync(id);

            var proof = _context.Proof.Include(u => u.ApplicationUser).Where(s => s.Email == cu_user.Email).ToList();
            return View(proof);
            //return View(await _context.Proof.ToListAsync());
        }


        // GET: Proofs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proof = await _context.Proof
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proof == null)
            {
                return NotFound();
            }

            return View(proof);
        }

        public async Task<IActionResult> GetUserProof(string email)
        {
            var usercurrent = await userManager.FindByIdAsync(User.getUserId());
            var proofid = _context.Proof.Where(s => s.Email.Contains(email) && s.Name == usercurrent.Email);
            return View(proofid);
        }
        public async Task<IActionResult> Approveda(string id)
        {
            ApprovedUser app = new ApprovedUser();
             
            // Current user id
            var data = _context.ApprovedUsers.Where(s => s.UserId == id).ToList();
            var usermdata = _context.ApprovedUsers.Where(s => s.UserId == id).Count();
            if (usermdata == 0)
            {
                app.ApproverId = User.getUserId();

                // User id
                app.UserId = id;

                _context.ApprovedUsers.Add(app);
                _context.SaveChanges();
            }
            else
            {
                foreach (var item in data)
                {
                    if (item.ApproverId != null)
                    {
                        if (item.ApproverId.Contains(User.getUserId()))
                        {
                            state = false;
                            assigned = false;
                            if (usermdata >= 9)
                            {
                                var user = await userManager.FindByIdAsync(item.UserId);
                                var userrole = await userManager.AddToRoleAsync(user, "InitAdmin");
                            }
                            else
                            {
                                RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            if (usermdata > 9)
                            {
                                var user = await userManager.FindByIdAsync(item.UserId);
                                var userrole = await userManager.AddToRoleAsync(user, "InitAdmin");
                                var userid = await userManager.FindByIdAsync(user.Id);
                                userid.ApprovalCount = 10;
                                var result = await userManager.UpdateAsync(userid);

                                if (result.Succeeded)
                                    return RedirectToAction("Index", "Home");
                            }
                            state = true;
                        }

                    }
                    else
                    {
                        if (usermdata > 9)
                        {
                            var user = await userManager.FindByIdAsync(item.UserId);
                            var userrole = await userManager.AddToRoleAsync(user, "InitAdmin");
                            var userid = await userManager.FindByIdAsync(user.Id);
                            userid.ApprovalCount = 10;
                            var result = await userManager.UpdateAsync(userid);

                            if (result.Succeeded)
                                return RedirectToAction("Index", "Home");
                        }
                        state = true;
                    }
                }


            }
            if (state == true && assigned == true)
            {
                app.ApproverId = User.getUserId();

                // User id
                app.UserId = id;

                _context.ApprovedUsers.Add(app);
                _context.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        // GET: Proofs/Create
        public IActionResult Create(string email)
        {
            ViewData["email"] = email;
            if (!User.IsInRole("InitAdmin"))
            {

                return View();
            }
            else if (User.IsInRole("SuperAdmin"))
            {
                return View();
            }
            else if (User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Admin");
            }
        }

        // POST: Proofs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ApplicationMemberId,FileType,FileSize,File")] Proof proof, IFormFile formFile)
        {
            if (formFile != null)
            {
                if (formFile.Length > 0)
                {
                    byte[] p1 = null;

                    using (var fs1 = formFile.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    if (ModelState.IsValid)
                    {
                        var userid = User.getUserId();
                        proof.ApplicationMemberId = userid;
                        var useremail = await userManager.GetUserAsync(User);
                        proof.Email = useremail.Email;
                        proof.FileType = formFile.ContentType;
                        proof.FileSize = formFile.Length.ToString();
                        proof.File = p1;

                        _context.Add(proof);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return View(proof);
        }

        // GET: Proofs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proof = await _context.Proof.FindAsync(id);
            if (proof == null)
            {
                return NotFound();
            }
            return View(proof);
        }

        // POST: Proofs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ApplicationMemberId,FileType,FileSize,File")] Proof proof)
        {
            if (id != proof.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proof);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProofExists(proof.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(proof);
        }

        // GET: Proofs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proof = await _context.Proof
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proof == null)
            {
                return NotFound();
            }

            return View(proof);
        }

        // POST: Proofs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proof = await _context.Proof.FindAsync(id);
            _context.Proof.Remove(proof);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProofExists(int id)
        {
            return _context.Proof.Any(e => e.Id == id);
        }
    }
}
