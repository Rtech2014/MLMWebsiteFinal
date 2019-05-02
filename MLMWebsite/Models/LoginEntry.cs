using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.Models
{
    public class LoginEntry
    {
        [Key]
        public int EntryID { get; set; }
        public string ApplicationMemberId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } //represents the user who was logged

        public DateTime? TimeOfLastLogin { get; set; } //time when the user logged last time

    }
}
