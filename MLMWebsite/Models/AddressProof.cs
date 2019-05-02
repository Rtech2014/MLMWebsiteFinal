using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.Models
{
    public class AddressProof
    {
        public int Id { get; set; }

        [Display(Name = "Address Proof")]
        public byte[] Address { get; set; }

        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
