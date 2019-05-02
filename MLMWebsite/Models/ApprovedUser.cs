using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.Models
{
    public class ApprovedUser
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string ApproverId { get; set; }
    }
}
