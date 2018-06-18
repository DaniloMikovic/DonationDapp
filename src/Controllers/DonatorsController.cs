using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChartJSCore.Models;
using DonacijeDapp.Models;
using DonacijeDapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DonacijeDapp.Controllers
{
    public class DonatorsController : Controller
    {
        private readonly BlockchainService _blockchainService;

        public DonatorsController(BlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _blockchainService.GetCampaigns();
            if (response.Succeeded)
            {
                return View(response.Value);
            }
            return BadRequest();
        }

        public async Task<IActionResult> Graph(string address)
        {
            var graphData = await _blockchainService.GetGraphData(address);
            if (graphData.Succeeded)
            {
                Chart chart = new Chart();
                chart.Type = "pie";

                ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
                data.Labels = new List<string>();

                var pieData = new List<double>();
                var pieColors = new List<string>();
                foreach (var item in graphData.Value)
                {
                    data.Labels.Add(item.Address);
                    pieData.Add(item.Donation);
                    pieColors.Add(item.Color);
                }

                PieDataset dataset = new PieDataset()
                {
                    Label = "Donatori",
                    Data = pieData,
                    BackgroundColor = pieColors,

                };

                data.Datasets = new List<Dataset>();
                data.Datasets.Add(dataset);
                chart.Data = data;
                ViewData["chart"] = chart;

                var response = await _blockchainService.GetCampaigns();

                var model = new GraphInitViewModel
                {
                    Campaigns = response.Value
                };

                return View(model);
            }
            return BadRequest();

        }

        public async Task<IActionResult> DonatorsList(string campaignAddress)
        {
            var response = await _blockchainService.DonatorsList(campaignAddress);
            if (response.Succeeded)
            {
                return View("_DonatorsListPartial", response.Value);
            }
            return BadRequest();
        }
    }
}