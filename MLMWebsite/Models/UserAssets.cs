using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.Models
{
    public class UserAssets
    {
        public int Id { get; set; }

        [Display(Name = "Profile Photo")]
        public byte[] ProfilePhoto { get; set; }

        public  string  UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
