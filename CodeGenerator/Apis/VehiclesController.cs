using Microsoft.AspNetCore.Mvc;
using Models;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController : ControllerBase
{
    [HttpGet]
    public void CreateVehicle(Vehicle vehicle)
    {
        Console.WriteLine("Creating a new vehicle...");
       
    }
}