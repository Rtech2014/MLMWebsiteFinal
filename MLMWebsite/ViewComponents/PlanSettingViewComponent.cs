using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.ViewComponents
{
    public class PlanSettingViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public PlanSettingViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var plan =  _context.LevelSettings.Where(s=>s.LevelName=="Mercury").ToList();

            return View(plan);
        }
    }
}
