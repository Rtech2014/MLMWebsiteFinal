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
    public class BarCodesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BarCodesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BarCodes
        public async Task<IActionResult> Index()
        {
            var c_User = User.getUserId();
            var data = _context.BarCodes.Include(b => b.ApplicationUser).Where(s => s.UserId == c_User);
            return View(data);
        }

        public async Task<IActionResult> ViewBar(string email)
        {
            var c_User = User.getUserId();
            var data = _context.BarCodes.Include(b => b.ApplicationUser).Where(s => s.ApplicationUser.Email.Contains(email));
            return View(data);
        }

        // GET: BarCodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barCode = await _context.BarCodes
                .Include(b => b.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (barCode == null)
            {
                return NotFound();
            }

            return View(barCode);
        }

        // GET: BarCodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BarCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QRCode,UserId")] BarCode barCode, IFormFile formFile)
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
                       barCode.UserId = userid;
                      
                       barCode.QRCode = p1;
                        _context.Add(barCode);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            
            return RedirectToAction("Index", "Home");
        }

        // GET: BarCodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barCode = await _context.BarCodes.FindAsync(id);
            if (barCode == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", barCode.UserId);
            return View(barCode);
        }

        // POST: BarCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QRCode,UserId")] BarCode barCode)
        {
            if (id != barCode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(barCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BarCodeExists(barCode.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", barCode.UserId);
            return View(barCode);
        }

        // GET: BarCodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barCode = await _context.BarCodes
                .Include(b => b.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (barCode == null)
            {
                return NotFound();
            }

            return View(barCode);
        }

        // POST: BarCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var barCode = await _context.BarCodes.FindAsync(id);
            _context.BarCodes.Remove(barCode);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create","Barcodes");
        }

        private bool BarCodeExists(int id)
        {
            return _context.BarCodes.Any(e => e.Id == id);
        }
    }
}
