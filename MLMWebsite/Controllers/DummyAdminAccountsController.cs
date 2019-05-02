using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MLMWebsite.Data;
using MLMWebsite.Models;

namespace MLMWebsite.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class DummyAdminAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DummyAdminAccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DummyAdminAccounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.DummyAdminAccounts.ToListAsync());
        }

        // GET: DummyAdminAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dummyAdminAccount = await _context.DummyAdminAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dummyAdminAccount == null)
            {
                return NotFound();
            }

            return View(dummyAdminAccount);
        }

        // GET: DummyAdminAccounts/Create
        public IActionResult Create()
        {
            if (_context.DummyAdminAccounts.Count()<10)
            {
                return View();
            }
            else
            {
                return RedirectToAction("DummyAccounterror","Admin");
            }
        }

        // POST: DummyAdminAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AccountNo,GooglePay,PhonePay")] DummyAdminAccount dummyAdminAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dummyAdminAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dummyAdminAccount);
        }

        // GET: DummyAdminAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dummyAdminAccount = await _context.DummyAdminAccounts.FindAsync(id);
            if (dummyAdminAccount == null)
            {
                return NotFound();
            }
            return View(dummyAdminAccount);
        }

        // POST: DummyAdminAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AccountNo,GooglePay,PhonePay")] DummyAdminAccount dummyAdminAccount)
        {
            if (id != dummyAdminAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dummyAdminAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DummyAdminAccountExists(dummyAdminAccount.Id))
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
            return View(dummyAdminAccount);
        }

        // GET: DummyAdminAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dummyAdminAccount = await _context.DummyAdminAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dummyAdminAccount == null)
            {
                return NotFound();
            }

            return View(dummyAdminAccount);
        }

        // POST: DummyAdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dummyAdminAccount = await _context.DummyAdminAccounts.FindAsync(id);
            _context.DummyAdminAccounts.Remove(dummyAdminAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DummyAdminAccountExists(int id)
        {
            return _context.DummyAdminAccounts.Any(e => e.Id == id);
        }
    }
}
