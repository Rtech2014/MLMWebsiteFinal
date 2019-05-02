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
    [ViewComponent(Name = "ProofApproval")]
    public class ProofApprovalViewComponent:ViewComponent
    {

        private readonly ApplicationDbContext _context;

        public ProofApprovalViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var proof = await _context.Proof.Include(s=>s.ApplicationUser).ToListAsync();
            return View(proof);
        }
    }
}
