using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OVRM.Data;
using OVRM.DTO;
using OVRM.Models;

namespace OVRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly OVRMDbContext _context;
        public VehiclesController(OVRMDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            return await _context.Vehicles.Where(v => v.IsAvailable).ToListAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Vehicle>> AddVehicle([FromBody] VehicleCreateDTO vehicleCreateDTO)
        {
            if (vehicleCreateDTO == null)
            {
                return BadRequest("Vehicle cannot be null");
            }

            // Map VehicleCreateDTO to Vehicle
            var vehicle = new Vehicle
            {
                Brand = vehicleCreateDTO.Brand,
                Model = vehicleCreateDTO.Model,
                Type = vehicleCreateDTO.Type,
                RentPerDay = vehicleCreateDTO.RentPerDay,
                IsAvailable = vehicleCreateDTO.IsAvailable
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicles), new { id = vehicle.Id }, vehicle);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleResponseDTO>> UpdateVehicle(int id, [FromBody] VehicleCreateDTO vehicleDto)
        {
            var existingVehicle = await _context.Vehicles.FindAsync(id);
            if (existingVehicle == null)
            {
                return NotFound("Vehicle not found");
            }

            // Update the existing vehicle's properties
            existingVehicle.Brand = vehicleDto.Brand;
            existingVehicle.Model = vehicleDto.Model;
            existingVehicle.Type = vehicleDto.Type;
            existingVehicle.RentPerDay = vehicleDto.RentPerDay;
            existingVehicle.IsAvailable = vehicleDto.IsAvailable;

            await _context.SaveChangesAsync();

            var vehicleResponse = new VehicleResponseDTO
            {
                Id = existingVehicle.Id,
                Brand = existingVehicle.Brand,
                Model = existingVehicle.Model,
                Type = existingVehicle.Type,
                RentPerDay = existingVehicle.RentPerDay,
                IsAvailable = existingVehicle.IsAvailable
            };

            return Ok(vehicleResponse);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return NotFound();

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return Ok($"Vehicle with ID {id} deleted.");
        }

    }
}
