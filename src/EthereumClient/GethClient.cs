using Nethereum.JsonRpc.IpcClient;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DonacijeDapp.EthereumClient
{
    public class GethClient : IGethClient
    {
        public Web3 Web3 { get; set; }
        public GethClient()
        {
            var ipcClient = new IpcClient("./geth.ipc");
            Web3 = new Web3(ipcClient);
        }
    }
}
