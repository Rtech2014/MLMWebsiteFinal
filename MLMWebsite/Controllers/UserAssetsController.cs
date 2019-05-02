using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MLMWebsite.Data;
using MLMWebsite.Models;

namespace MLMWebsite.Controllers
{
    public class UserAssetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserAssetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserAssets
        public async Task<IActionResult> Index()
        {
            var c_User = User.getUserId();
            var data = _context.UserAssets.Include(b => b.ApplicationUser).Where(s => s.UserId == c_User);
            return View(data);
        }
            // GET: UserAssets/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAssets = await _context.UserAssets
                .Include(u => u.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAssets == null)
            {
                return NotFound();
            }

            return View(userAssets);
        }

        // GET: UserAssets/Create
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: UserAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProfilePhoto,UserId")] UserAssets userAssets, IFormFile formFile)
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
                        userAssets.UserId = userid;

                        userAssets.ProfilePhoto = p1;
                        _context.Add(userAssets);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index","Home");
                    }
                }
            }


            return View(userAssets);
        }

        // GET: UserAssets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAssets = await _context.UserAssets.FindAsync(id);
            if (userAssets == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userAssets.UserId);
            return View(userAssets);
        }

        // POST: UserAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePhoto,UserId")] UserAssets userAssets)
        {
            if (id != userAssets.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAssets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAssetsExists(userAssets.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userAssets.UserId);
            return View(userAssets);
        }

        // GET: UserAssets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAssets = await _context.UserAssets
                .Include(u => u.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAssets == null)
            {
                return NotFound();
            }

            return View(userAssets);
        }

        // POST: UserAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userAssets = await _context.UserAssets.FindAsync(id);
            _context.UserAssets.Remove(userAssets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAssetsExists(int id)
        {
            return _context.UserAssets.Any(e => e.Id == id);
        }
    }
}
