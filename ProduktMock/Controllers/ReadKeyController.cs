using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ProduktMock.Models;

namespace ProduktMock.Controllers
{
    [Route("api/[Controller]")]
    public class ReadKeyController
    {
        [HttpGet("{id}")]
        public IActionResult GetActionResult(int id)
        {
            string location = "Vault/SUPERSECRETKEY%keyNumber%.txt".Replace("%keyNumber%", id.ToString());
            string result = System.IO.File.ReadAllText(location);
            return new OkObjectResult(result);
        }
    }
 }