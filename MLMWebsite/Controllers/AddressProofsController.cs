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
    public class AddressProofsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddressProofsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AddressProofs
        public async Task<IActionResult> Index()
        {
            var c_User = User.getUserId();
            var data = _context.AddressProofs.Include(b => b.ApplicationUser).Where(s => s.UserId == c_User);
            return View(data);
        }

        // GET: AddressProofs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressProof = await _context.AddressProofs
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addressProof == null)
            {
                return NotFound();
            }

            return View(addressProof);
        }

        // GET: AddressProofs/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: AddressProofs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,UserId")] AddressProof addressProof ,IFormFile formFile)
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
                        addressProof.UserId = userid;

                        addressProof.Address = p1;
                        _context.Add(addressProof);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index","Home");
                    }
                }
            }

           
            return View(addressProof);
        }

        // GET: AddressProofs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressProof = await _context.AddressProofs.FindAsync(id);
            if (addressProof == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", addressProof.UserId);
            return View(addressProof);
        }

        // POST: AddressProofs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,UserId")] AddressProof addressProof)
        {
            if (id != addressProof.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(addressProof);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressProofExists(addressProof.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", addressProof.UserId);
            return View(addressProof);
        }

        // GET: AddressProofs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressProof = await _context.AddressProofs
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addressProof == null)
            {
                return NotFound();
            }

            return View(addressProof);
        }

        // POST: AddressProofs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var addressProof = await _context.AddressProofs.FindAsync(id);
            _context.AddressProofs.Remove(addressProof);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressProofExists(int id)
        {
            return _context.AddressProofs.Any(e => e.Id == id);
        }
    }
}
