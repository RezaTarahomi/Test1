using System;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace CodeGenerator.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class Vehicle_TestController : ControllerBase
    {
        public int _unitOfWork { get; set; }

        public Vehicle_TestController(int unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetVehicle_Tests2()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateVehicle_Tests2()
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult GetVehicle_Tests()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetVehicle_Tests54()
        {
            return Ok();
        }
    }
}