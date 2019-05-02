using MLMWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.Models
{
    public class UserStatus
    {
         public int ID { get; set; }

        public string ApproverID { get; set; }

        public int ProofID { get; set; }
        public Proof Proof { get; set; }


    }
}
