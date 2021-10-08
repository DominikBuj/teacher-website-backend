using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Data;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;
using TeacherWebsiteBackEnd.DTOs;

namespace TeacherWebsiteBackEnd.Controllers
{
    [Route("api/dissertations")]
    [ApiController]
    public class DissertationsController : Controller
    {
        private readonly IDissertationService _dissertationsService;

        public DissertationsController(IDissertationService dissertationsService)
        {
            _dissertationsService = dissertationsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DissertationForm>>> GetDissertations()
        {
            IEnumerable<DissertationForm> dissertations = await _dissertationsService.GetDissertations();
            if (dissertations == null) return NotFound();
            return Ok(dissertations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DissertationForm>> GetDissertationById(int id)
        {
            DissertationForm dissertation = await _dissertationsService.GetDissertationById(id);
            if (dissertation == null) return NotFound();
            return Ok(dissertation);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<DissertationForm>> AddDissertation([FromBody] DissertationForm dissertation)
        {
            DissertationForm _dissertation = await _dissertationsService.AddDissertation(dissertation);
            if (_dissertation == null) return BadRequest();
            return Created($"api/dissertations/{_dissertation.Id}", _dissertation);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<DissertationForm>> ReplaceDissertation([FromBody] DissertationForm dissertation)
        {
            DissertationForm _dissertation = await _dissertationsService.ReplaceDissertation(dissertation);
            if (_dissertation == null) return BadRequest();
            return Ok(_dissertation);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDissertationById(int id)
        {
            bool success = await _dissertationsService.DeleteDissertationById(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
