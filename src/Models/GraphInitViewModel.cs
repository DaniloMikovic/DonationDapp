using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonacijeDapp.Models
{
    public class GraphInitViewModel
    {
        public string Campaign { get; set; }
        public List<CampaignModel> Campaigns { get; set; }
    }
}
