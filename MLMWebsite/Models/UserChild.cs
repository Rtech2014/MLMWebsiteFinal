using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.Models
{
    public class UserChild
    {
        public int Id { get; set; }
        public string ChildId { get; set; }

        public string ParentUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
