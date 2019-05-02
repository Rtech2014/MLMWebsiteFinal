using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.Models
{
    public class BarCode
    {
        public int Id { get; set; }

        [Display(Name = "QRCode")]
        public byte[] QRCode { get; set; }

        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
