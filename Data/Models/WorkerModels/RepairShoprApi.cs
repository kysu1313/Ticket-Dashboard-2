using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;
using HUD.Data.Models.UserModels;

namespace HUD.Models.RepairShopr
{
    public class RepairShoprApi
    {

        private string _urlHeader = "https://";
        private string _urlRoute = ".repairshopr.com/api/v1/tickets?page=";
        private string _settingsRoute = ".repairshopr.com/api/v1/settings";
        private int _pageNum;
        public string _apiKey { get; set; }
        public string _urlPrefix { get; set; }
        private IConfiguration Configuration;


        /// <summary>
        /// Set configuration for API key access
        /// </summary>
        /// <param name="configuration"></param>
        public RepairShoprApi(IConfiguration configuration, string urlPrefix, string apiKey)
        {
            Configuration = configuration;
            _apiKey = apiKey;
            _urlPrefix = urlPrefix;
        }

        /// <summary>
        /// Returns the list of tickets from the RS API
        /// </summary>
        /// <returns></returns>
        public List<RepairTicket> GetTicketList()
        {
            List<RepairTicket> fullList = new List<RepairTicket>();
            string ticket_url = _urlHeader + _urlPrefix + _urlRoute;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add("Content-Type", "application/json; charset=utf-8"); //; charset=utf-8
                    webClient.Headers.Add("Authorization", _apiKey);
                    JObject job;
                    _pageNum = 1;
                    int pages;
                    do
                    {
                        webClient.BaseAddress = ticket_url + _pageNum++;
                        var json = webClient.DownloadString("");
                        job = JObject.Parse(json);
                        pages = (int)job.SelectToken("meta").SelectToken("total_pages");
                        fullList.AddRange(ParseJson(job));
                    } while (_pageNum <= pages);


                    return fullList;
                }
            }
            catch (WebException ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Parse the json result from API call into list of Ticket objects
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        private List<RepairTicket> ParseJson(JObject job)
        {
            // Prevent null values in response
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            List<RepairTicket> ri;

            try
            {
                ri = JsonConvert.DeserializeObject<List<RepairTicket>>(job.SelectToken("tickets").ToString(), settings);
            }
            catch (NullReferenceException ex)
            {
                throw;
            }

            return ri;
        }

        public Dictionary<string, string> ApiGetSettings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            string settings_url = _urlHeader + _urlPrefix + _settingsRoute;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add("Content-Type", "application/json; charset=utf-8"); //; charset=utf-8
                    webClient.Headers.Add("Authorization", _apiKey);
                    JObject job;
                    //_pageNum = 1;
                    //int pages;
                    //do
                    //{
                    webClient.BaseAddress = settings_url;
                    var json = webClient.DownloadString("");
                    job = JObject.Parse(json);
                    Debug.WriteLine(job);
                    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(job.SelectToken("profile").ToString());
                    foreach (var v in data)
                    {
                        settings.Add(v.Key, v.Value);
                    }

                    return settings;
                }
            }
            catch (WebException ex)
            {
                throw;
            }
        }


    }
}
