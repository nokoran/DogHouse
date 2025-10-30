using Dogshouseservice.Api.DTOs;
using Dogshouseservice.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dogshouseservice.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogsController : ControllerBase
    {
        private readonly IDogService _dogService;
        public DogsController(IDogService dogService)
        {
            _dogService = dogService;
        }
        
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Dogshouseservice.Version1.0.1");
        }
        
        [HttpGet("dogs")]
        public async Task<IActionResult> GetDogs([FromQuery] PaginationQuery query)
        {
            var dogs = await _dogService.GetDogsAsync(query);
            return Ok(dogs);
        }
        
        [HttpPost("dog")]
        public async Task<IActionResult> CreateDog([FromBody] CreateDogRequest request)
        {
            try
            {
                await _dogService.CreateDogAsync(request);
                return StatusCode(201, "Dog created successfully");
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An internal error occurred");
            }
        }
    }
}
