using Microsoft.AspNetCore.Mvc;
using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.ViewComponents
{
    public class AddressProofViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AddressProofViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var AddressProof = _context.AddressProofs.ToList();
            return View(AddressProof);
        }
    }
}
