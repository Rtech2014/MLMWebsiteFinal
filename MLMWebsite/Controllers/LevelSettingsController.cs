using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MLMWebsite.Data;
using MLMWebsite.Models;

namespace MLMWebsite.Controllers
{
    public class LevelSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LevelSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Mercury()
        {
            var plan = await _context.LevelSettings
                .Where(s => s.LevelName == "Mercury").ToListAsync();
            return View(plan);
        }

        public async Task<IActionResult> Venus()
        {
            var plan = await _context.LevelSettings
               .Where(s => s.LevelName == "Venus").ToListAsync();
            return View(plan);

        }

        public async Task<IActionResult> Jupiter()
        {
            var plan = await _context.LevelSettings
               .Where(s => s.LevelName == "Jupiter").ToListAsync();
            return View(plan);

        }
        // GET: LevelSettings
        public async Task<IActionResult> Index()
        {
            return View(await _context.LevelSettings.ToListAsync());
        }

        // GET: LevelSettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var levelSetting = await _context.LevelSettings
                .FirstOrDefaultAsync(m => m.LevelId == id);
            if (levelSetting == null)
            {
                return NotFound();
            }

            return View(levelSetting);
        }

        // GET: LevelSettings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LevelSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LevelId,LevelName,PackagePrice,Level1,Level2,Level3,Level4,Level5,Level6,Level7,Level8,Level9,Level10,Admin")] LevelSetting levelSetting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(levelSetting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(levelSetting);
        }

        // GET: LevelSettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var levelSetting = await _context.LevelSettings.FindAsync(id);
            if (levelSetting == null)
            {
                return NotFound();
            }
            return View(levelSetting);
        }

        // POST: LevelSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LevelId,LevelName,PackagePrice,Level1,Level2,Level3,Level4,Level5,Level6,Level7,Level8,Level9,Level10,Admin")] LevelSetting levelSetting)
        {
            if (id != levelSetting.LevelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(levelSetting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LevelSettingExists(levelSetting.LevelId))
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
            return View(levelSetting);
        }

        // GET: LevelSettings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var levelSetting = await _context.LevelSettings
                .FirstOrDefaultAsync(m => m.LevelId == id);
            if (levelSetting == null)
            {
                return NotFound();
            }

            return View(levelSetting);
        }

        // POST: LevelSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var levelSetting = await _context.LevelSettings.FindAsync(id);
            _context.LevelSettings.Remove(levelSetting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LevelSettingExists(int id)
        {
            return _context.LevelSettings.Any(e => e.LevelId == id);
        }
    }
}
