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
    [Route("api/links")]
    [ApiController]
    public class LinksController : Controller
    {
        private readonly ILinkService _linksService;

        public LinksController(ILinkService linksService)
        {
            _linksService = linksService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LinkForm>>> GetLinks()
        {
            IEnumerable<LinkForm> links = await _linksService.GetLinks();
            if (links == null) return NotFound();
            return Ok(links);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LinkForm>> GetLinkById(int id)
        {
            LinkForm link = await _linksService.GetLinkById(id);
            if (link == null) return NotFound();
            return Ok(link);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<LinkForm>> AddLink([FromBody] LinkForm link)
        {
            LinkForm _link = await _linksService.AddLink(link);
            if (_link == null) return BadRequest();
            return Created($"api/links/{_link.Id}", _link);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<LinkForm>> ReplaceLink([FromBody] LinkForm link)
        {
            LinkForm _link = await _linksService.ReplaceLink(link);
            if (_link == null) return NotFound();
            return Ok(_link);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLinkById(int id)
        {
            bool success = await _linksService.DeleteLinkById(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
