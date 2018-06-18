using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace DonacijeDapp.Models
{
    public class DonationInfoViewModel
    {
        [Display(Name ="Početak")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Kraj")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Organizator")]
        public string Owner { get; set; }
        [Display(Name = "Primalac donacije")]
        public string Beneficiary { get; set; }
        [Display(Name = "Cilj")]
        public BigInteger DonationGoal { get; set; }
        [Display(Name = "Prikupljena suma")]
        public BigInteger DonationAmount { get; set; }
        [Display(Name = "Broj donatora")]
        public BigInteger NumberOfDonators { get; set; }

        public List<CampaignModel> Campaigns { get; set; }
    }
    
    
}
