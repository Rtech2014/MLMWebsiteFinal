using Microsoft.AspNetCore.Mvc;
using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.ViewComponents
{
    public class JupiterViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public JupiterViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var plan = _context.LevelSettings.Where(s => s.LevelName == "Jupiter").ToList();

            return View(plan);
        }
    }
}
