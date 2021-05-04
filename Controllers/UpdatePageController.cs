//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Newtonsoft.Json.Linq;
//using System.Text.Json;
//using System.Diagnostics;
//using Microsoft.Extensions.Configuration;
//using HUD.Models.RepairShopr.Api;
//using HUD.Data.Models;
//using HUD.Models;

//namespace HUD.Controllers
//{
//    //[Route("api/[controller]")]
//    [ApiController]
//    public class UpdatePageController : ControllerBase
//    {

//        private readonly IHubContext<UpdateHub> _hubContext;
//        private IConfiguration _config;

//        public UpdatePageController(IHubContext<UpdateHub> hubContext, IConfiguration ifconfig)
//        {
//            _hubContext = hubContext;
//            _config = ifconfig;
//        }

//        /// <summary>
//        /// Returns string posted to url: https://localhost:44372/api/UpdatePage?title=
//        /// </summary>
//        /// <param name="title"></param>
//        /// <returns></returns>
//        //[HttpPost]
//        //public async Task<IActionResult> Post([FromQuery]string title)
//        //{
//        //    await _hubContext.Clients.All.SendAsync("update", $"{DateTime.Now}: {title}");
//        //    return Ok("Mesasge sent successfully");
//        //}

//        private int GetTestNumber()
//        {
//            return new Random().Next(5, 50);
//        }

//        /// <summary>
//        /// Using random number for testing puroposes.
//        /// </summary>
//        /// <param name="num"></param>
//        /// <returns></returns>
//        //[Route("/random")]
//        [HttpGet]
//        [Route("/[action]")]
//        public async Task<IActionResult> RandomNumber()
//        {
//            List<int> nums = new List<int>();
//            for (int i = 0; i < 100; i++)
//            {
//                nums.Add(new Random().Next(234, 547));
//            }
//            var json = JsonSerializer.Serialize(nums);

//            await _hubContext.Clients.All.SendAsync("getnum", json);
//            return Ok("Mesasge sent successfully");
//        }

//        /// <summary>
//        /// Return list of test tickets.
//        /// This will be used to connect to the actual api.
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [Route("/[action]")]
//        public async Task<IActionResult> GetTickets()
//        {
//            Models.RepairShoprApi rsa = new Models.RepairShoprApi(_config);
//            List<RepairTicket> lst = rsa.GetTicketList();

//            var json = JsonSerializer.Serialize(lst);
//            await _hubContext.Clients.All.SendAsync("gettickets", json);
//            return Ok("Data sent successfully");
//        }

//        [HttpPost]
//        [Route("/[action]")]
//        public async Task<IActionResult> GetMainData()
//        {

//            // testing
//            Models.RepairShoprApi rsa = new Models.RepairShoprApi(_config);
//            List<RepairTicket> lst = rsa.GetTicketList();

//            Dictionary<string, int> dataDict = new Dictionary<string, int>();
//            dataDict.Add("incoming", new Random().Next(5, 25));
//            dataDict.Add("ready-to-repair", new Random().Next(5, 25));
//            dataDict.Add("waiting-on-customer", new Random().Next(20, 45));
//            dataDict.Add("needs-attention", new Random().Next(3, 10));
//            dataDict.Add("on-bench", new Random().Next(3, 10));
//            dataDict.Add("rush", new Random().Next(1, 5));

//            var json = JsonSerializer.Serialize(dataDict);

//            await _hubContext.Clients.All.SendAsync("getmaindata", json);
//            return Ok("Data sent successfully");
//        }

//        [HttpGet]
//        [Route("/[action]")]
//        public async Task<IActionResult> GetMainDataLongIncrement()
//        {
//            Dictionary<string, int> dataDict = new Dictionary<string, int>();
//            dataDict.Add("incoming", new Random().Next(5, 25));
//            dataDict.Add("ready-to-repair", new Random().Next(5, 25));
//            dataDict.Add("waiting-on-customer", new Random().Next(20, 45));
//            dataDict.Add("needs-attention", new Random().Next(3, 10));
//            dataDict.Add("on-bench", new Random().Next(3, 10));
//            dataDict.Add("rush", new Random().Next(1, 5));

//            var json = JsonSerializer.Serialize(dataDict);

//            await _hubContext.Clients.All.SendAsync("getmaindatadelay", json);
//            return Ok("Data sent successfully");
//        }
//    }
//}
