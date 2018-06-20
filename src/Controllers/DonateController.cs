using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DonacijeDapp.Config;
using DonacijeDapp.Models;
using DonacijeDapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DonacijeDapp.Controllers
{
    public class DonateController : Controller
    {
        private readonly BlockchainService _blockchainService;

        public DonateController(BlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
        }

        public async Task<IActionResult> Index()
        {
            var campaings = await _blockchainService.GetCampaigns();

            var model = new DonateViewModel
            {
                Campaigns = campaings.Value
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Donate(DonateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _blockchainService.Donate(model);
                return Json(response);
            }
            return Json("Fail");
        }

        [HttpGet]
        public async Task<IActionResult> GetEstimatedFee(DonateViewModel model)
        {
            var response = await _blockchainService.GetEstimatedFee(model);

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetBalance(string address)
        {
            var response = await _blockchainService.GetBalance(address);

            return Json(response);
        }
    }
}