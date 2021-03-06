using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WinTrackWebApp.Data;
using WinTrackWebApp.Models;

namespace WinTrackWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WintrackContext _context;
        private Arduino _arduino;
        public Arduino Arduino 
        { 
            get 
            {
                if (_arduino == null) _arduino = _context.Arduino.Find(1);
                return _arduino;
            } 
            set 
            {
                _arduino = value;  
            } 
        }

        public HomeController(ILogger<HomeController> logger, WintrackContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string trackStatus = "";
            if (Arduino.GetConnection()) { trackStatus += "On"; }
            else { trackStatus += "Off"; }
            if (Arduino.GetTrackStatus()) { trackStatus += "Right"; }
            else { trackStatus += "Left"; }
            
            ViewData["TrackStatus"] = trackStatus;
            ViewData["DemoData"] = Arduino.DemoData;
            ViewData["ServerUrl"] = Startup.ServerUrl;
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SwitchTrack()
        {
            Arduino.SwitchTrack();
            _context.Arduino.Update(Arduino);
            _context.SaveChanges();
            return StatusCode(200);
        }

        [HttpPost]
        public IActionResult SwitchData()
        {
            Arduino.DemoData = !Arduino.DemoData;
            _context.Arduino.Update(Arduino);
            _context.SaveChanges();
            return StatusCode(200);
        }

        public ActionResult<bool> GetTrackStatus()
        {
            return Arduino.GetTrackStatus();
        }

        public ActionResult<bool> GetConnection()
        {
            return Arduino.GetConnection();
        }
    }
}
