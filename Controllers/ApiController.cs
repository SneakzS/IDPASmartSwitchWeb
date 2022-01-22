using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;

using SmartSwitchWeb.Data;
using SmartSwitchWeb.Database;
using SmartSwitchWeb.Handlers;
using SmartSwitchWeb;
using System.Net.WebSockets;

namespace SmartSwitchWeb.Controllers
{
    public class ApiController : ControllerBase
    {
        [Route("/api/v1/{**rest}")]
        public IActionResult Fallback()
        {
            return NotFound();
        }
    }
}