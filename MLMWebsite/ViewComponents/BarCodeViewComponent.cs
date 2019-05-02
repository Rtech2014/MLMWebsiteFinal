using Microsoft.AspNetCore.Mvc;
using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.ViewComponents
{
    public class BarCodeViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public BarCodeViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var AddressProof = _context.BarCodes.ToList();
            return View(AddressProof);
        }
    }
}
