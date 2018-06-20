using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DonacijeDapp.Models;
using Microsoft.Extensions.Options;
using DonacijeDapp.Config;
using DonacijeDapp.EthereumClient;
using Nethereum.Hex.HexTypes;
using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3.Accounts;
using Nethereum.Web3;
using Nethereum.KeyStore;
using System.IO;

namespace DonacijeDapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<MyConfig> _config;
        private readonly IGethClient _gethClient;

        public HomeController(IOptions<MyConfig> config, IGethClient gethClient)
        {
            _config = config;
            _gethClient = gethClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SmartContract()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
