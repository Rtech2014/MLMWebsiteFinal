using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.Models
{
    public class Proof
    {
        public int Id { get; set; }
        [Display(Name = "Description")]
        public string Name { get; set; }
        public string ApplicationMemberId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string Email { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }

        [Display(Name = "Upload Transaction Proof")]
        public byte[] File { get; set; }
    }
}
