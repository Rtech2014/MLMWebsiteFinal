using Microsoft.AspNetCore.Identity;
using MLMWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Proofs = new HashSet<Proof>();
            LoginEntries = new HashSet<LoginEntry>();
            this.JoinDate = DateTime.Now.Date;
        }

        public virtual ICollection<Proof> Proofs { get; set; }
        public virtual ICollection<LoginEntry> LoginEntries { get; set; }
        public virtual UserAssets UserAssets { get; set; }
        public virtual AddressProof AddressProofs { get; set; }
        public virtual BarCode BarCodes { get; set; }
        public virtual ICollection<UserChild> UserChildren { get; set; }
        public string LeftBranchID { get; set; }
        public string RightBranchID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RecordCount { get; set; }
        public string Pathfile { get; set; }
        public int branchcount { get; set; }
        public string ParentID { get; set; }
        public int ApprovalCount { get; set; }
        public DateTime JoinDate { get; set; }

        // Bank Details
        [Display(Name = "Account No")]
        [PersonalData]
        public string AccountNo { get; set; }

        [Display(Name = "IFSC Code")]
        [PersonalData]
        public string IFSC_Code { get; set; }

        [Display(Name = "Address")]
        [PersonalData]
        public string Address { get; set; }

        [Display(Name = "Bank Name")]
        [PersonalData]
        public string Bank { get; set; }


        // Personal Details
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }
        
        public string GooglePay { get; set; }
       
        public string PhonePay { get; set; }
    }
}
