using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using DonacijeDapp.Config;
using DonacijeDapp.EthereumClient;
using DonacijeDapp.Models;
using DonacijeDapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DonacijeDapp.Controllers
{
    public class InfoController : Controller
    {
        private readonly IOptions<MyConfig> _config;
        private readonly IGethClient _gethClient;
        private readonly BlockchainService _blockchainService;

        public InfoController(IOptions<MyConfig> config, IGethClient gethClient, BlockchainService blockchainService)
        {
            _config = config;
            _gethClient = gethClient;
            _blockchainService = blockchainService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Token()
        {
            var contract = _gethClient.Web3.Eth.GetContract(_config.Value.AbiToken, _config.Value.TokenContractAddress);

            try
            {
                var model = new TokenInfoViewModel()
                {
                    Name = await contract.GetFunction("name").CallAsync<string>(),
                    Symbol = await contract.GetFunction("symbol").CallAsync<string>(),
                    TotalSupply = await contract.GetFunction("totalSupply").CallAsync<BigInteger>(),
                    Decimals = await contract.GetFunction("decimals").CallAsync<int>(),
                };

                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Donation()
        {
            var campaigns = await _blockchainService.GetCampaigns();
            if (campaigns.Succeeded)
            {
                return View(campaigns.Value);
            }
            return BadRequest();
        }

        public async Task<IActionResult> GetCampaignInfo(string address)
        {
            var campaign = await _blockchainService.GetCampaignInfo(address);
            if (campaign.Succeeded)
            {
                return PartialView("_CampaignInfoPartial", campaign.Value);
            }
            return BadRequest();
        }
    }
}