using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonacijeDapp.EthereumClient
{
    public interface IGethClient
    {
        Web3 Web3 { get; set; }
    }
}
