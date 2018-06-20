using DonacijeDapp.Config;
using DonacijeDapp.EthereumClient;
using DonacijeDapp.Models;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DonacijeDapp.Services
{
    public class BlockchainService
    {
        private readonly IGethClient _gethClient;
        private readonly IOptions<MyConfig> _config;

        public BlockchainService(IGethClient gethClient, IOptions<MyConfig> config)
        {
            _gethClient = gethClient;
            _config = config;
        }

        public async Task<Response<List<CampaignModel>>> GetCampaigns()
        {
            var response = new Response<List<CampaignModel>>
            {
                Value = new List<CampaignModel>()
            };

            var storageContract = _gethClient.Web3.Eth.GetContract(_config.Value.AbiCampaignStorage, _config.Value.CampaingStorageContractAddress);
            var length = 1;
            try
            {
                for (int i = 0; i < length; i++)
                {
                    var campaign = await storageContract.GetFunction("getCampaign").CallAsync<string>(i);
                    if (campaign != string.Empty)
                    {
                        var campaignModel = campaign.Split(",");
                        response.Value.Add(new CampaignModel
                        {
                            Address = campaignModel[0],
                            Name = campaignModel[1]
                        });
                        length++;
                    }
                    else
                    {
                        break;
                    }


                }
                response.Succeeded = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Succeeded = false;
            }


            return response;
        }

        public async Task<Response<string>> Donate(DonateViewModel model)
        {
            var response = new Response<string>();

            try
            {
                
                var txCount = await _gethClient.Web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(model.Address);
                var encoded = Web3.OfflineTransactionSigner.SignTransaction(model.PrivateKey, model.Campaign, Web3.Convert.ToWei(new BigInteger(model.Amount)), txCount.Value,new BigInteger(40000000000), new BigInteger(400000));
                var txHash = await _gethClient.Web3.Eth.Transactions.SendRawTransaction.SendRequestAsync("0x" + encoded);

                response.Value = txHash;
                response.Succeeded = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Succeeded = false;
            }

            return response;
        }

        public async Task<Response<List<DonationModel>>> DonatorsList(string campaignAddress)
        {
            var response = new Response<List<DonationModel>>();

            try
            {
                var contract = _gethClient.Web3.Eth.GetContract(_config.Value.AbiDonation, campaignAddress);
                var numberOfDonators = await contract.GetFunction("totalNumberOfDonatos").CallAsync<BigInteger>();

                for (var i = 0; i < numberOfDonators; i++)
                {
                    var donator = await contract.GetFunction("donators").CallAsync<string>(i);
                    var donation = await contract.GetFunction("balanceOf").CallAsync<BigInteger>(donator);
                    response.Value.Add(new DonationModel
                    {
                        Address = donator,
                        Donation = donation
                    });
                }
                response.Succeeded = true;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Succeeded = false;
            }


            return response;
        }

        public async Task<Response<DonationInfoViewModel>> GetCampaignInfo(string address)
        {
            var response = new Response<DonationInfoViewModel>();
            try
            {
                var donationContract = _gethClient.Web3.Eth.GetContract(_config.Value.AbiDonation, address);
                
                var start = await donationContract.GetFunction("startTime").CallAsync<BigInteger>();
                var end = await donationContract.GetFunction("endTime").CallAsync<BigInteger>();

                var startFormated = start.UnixTimeStampToDateTime();
                var endFormated = end.UnixTimeStampToDateTime();

                var donationInfo = new DonationInfoViewModel()
                {
                    NumberOfDonators = await donationContract.GetFunction("totalNumOfDonators").CallAsync<BigInteger>(),
                    StartDate = startFormated,
                    EndDate = endFormated,
                    Owner = await donationContract.GetFunction("owner").CallAsync<string>(),
                    Beneficiary = await donationContract.GetFunction("beneficiary").CallAsync<string>(),
                    DonationGoal = await donationContract.GetFunction("donationGoal").CallAsync<BigInteger>(),
                    DonationAmount = await donationContract.GetFunction("amountRaised").CallAsync<BigInteger>()
                };
                response.Succeeded = true;
                response.Value = donationInfo;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Succeeded = false;
            }

            return response;
        }

        public async Task<Response<string>> GetBalance(string account)
        {
            var response = new Response<string>();

            try
            {
                var balance = await _gethClient.Web3.Eth.GetBalance.SendRequestAsync(account);
                response.Succeeded = true;
                response.Value = Math.Round(Web3.Convert.FromWei(balance.Value), 8) + " Eth";
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<Response<string>> GetEstimatedFee(DonateViewModel model)
        {
            var response = new Response<string>();
            var test = model.Amount;
            var callInput = new CallInput()
            {
                From = model.Address,
                To = model.Campaign,
                Value = new HexBigInteger(new BigInteger(model.Amount))
            };
            try
            {

                var gas = await _gethClient.Web3.Eth.Transactions.EstimateGas.SendRequestAsync(callInput);

                var gasPrice = await _gethClient.Web3.Eth.GasPrice.SendRequestAsync();
                var gasPriceEth = Web3.Convert.FromWei(gasPrice);

                var estimatedFee = (decimal)gas.Value * gasPriceEth;

                response.Value = estimatedFee + " Eth";
                response.Succeeded = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Succeeded = false;
            }

            return response;
        }

        public async Task<Response<List<GraphViewModel>>> GetGraphData(string address)
        {
            var contract = _gethClient.Web3.Eth.GetContract(_config.Value.AbiDonation, address);

            var response = new Response<List<GraphViewModel>>()
            {
                Value = new List<GraphViewModel>()
            };
            try
            {
                var numberOfDonators = await contract.GetFunction("totalNumOfDonators").CallAsync<BigInteger>();


                for (BigInteger i = 0; i < numberOfDonators; i++)
                {
                    var graphData = new GraphViewModel();

                    var donator = await contract.GetFunction("donators").CallAsync<string>(i);
                    var donation = await contract.GetFunction("balanceOf").CallAsync<BigInteger>(donator);
                    var random = new Random();
                    var color = String.Format("#{0:X6}", random.Next(0x1000000));

                    graphData.Address = donator;
                    graphData.Donation = (double)donation;
                    graphData.Color = color;

                    response.Value.Add(graphData);
                    response.Succeeded = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Succeeded = false;
            }


            return response;
        }

    }
}
