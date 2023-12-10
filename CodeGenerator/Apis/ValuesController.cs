
using System;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace CodeGenerator.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class Vehicle_Test2Controller : ControllerBase
    {
        public int _unitOfWork { get; set; }

        public Vehicle_Test2Controller(int unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetVehicle_Tests()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateVehicle_Tests()
        {
            return Ok();
        }
    }
}