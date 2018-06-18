using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DonacijeDapp.Models
{
    public class DonateViewModel
    {
        [Required]
        [Display(Name = "Kampanja:")]
        public string Campaign { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Račun:")]
        public string Address { get; set; }
        
        [Display(Name = "Stanje:")]
        public string Balance { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Privatni ključ:")]
        public string PrivateKey { get; set; }

        [Required]
        [Display(Name = "Iznos:")]
        public decimal Amount { get; set; }
        
        [Display(Name = "Procenjena provizija:")]
        public string Fee { get; set; }
        
        public List<CampaignModel> Campaigns { get; set; }
    }
}
