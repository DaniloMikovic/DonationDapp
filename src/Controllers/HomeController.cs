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
        private readonly IOptions<Myconfig> _config;
        private readonly IGethClient _gethClient;

        public HomeController(IOptions<Myconfig> config, IGethClient gethClient)
        {
            _config = config;
            _gethClient = gethClient;
        }
        public IActionResult Index()
        {

            //var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            //var privateKey = ecKey.GetPrivateKey();
            //var account = new Account(privateKey);


            //var keyStoreService = new KeyStoreService();

            //var fileName = keyStoreService.GenerateUTCFileName(account.Address);
            //using (var newfile = System.IO.File.CreateText(fileName))
            //{
            //    //generate the encrypted and key store content as json. (The default uses pbkdf2)
            //    var newJson = keyStoreService.EncryptAndGenerateDefaultKeyStoreAsJson("NSWD601069", account.PrivateKey.HexToByteArray(), account.Address);
            //    newfile.Write(newJson);
            //    newfile.Flush();
            //}



            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
