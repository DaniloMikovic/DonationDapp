using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace DonacijeDapp.Models
{
    public class TokenInfoViewModel
    {
        [Display(Name = "Naziv:")]
        public string Name { get; set; }
        [Display(Name = "Simbol:")]
        public string Symbol { get; set; }
        [Display(Name = "Ukupna količina tokena:")]
        public BigInteger TotalSupply{ get; set; }
        [Display(Name = "Decimalna mesta:")]
        public int Decimals { get; set; }
    }
}
