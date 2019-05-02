using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLMWebsite.Models
{
    public class LevelSetting
    {
        [Key]
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public string PackagePrice { get; set; }
        public double Level1  { get; set; }
        public double Level2 { get; set; }
        public double Level3 { get; set; }
        public double Level4 { get; set; }
        public double Level5 { get; set; }
        public double Level6 { get; set; }
        public double Level7 { get; set; }
        public double Level8 { get; set; }
        public double Level9 { get; set; }
        public double Level10 { get; set; }
        public double Admin { get; set; }
    }
}
