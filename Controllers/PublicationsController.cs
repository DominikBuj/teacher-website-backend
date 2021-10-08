using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Data;
using TeacherWebsiteBackEnd.Models;
using TeacherWebsiteBackEnd.DTOs;

namespace TeacherWebsiteBackEnd.Controllers
{
    [Route("api/publications")]
    [ApiController]
    public class PublicationsController : Controller
    {
        private readonly IPublicationService _publicationService;

        public PublicationsController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicationForm>>> GetPublications()
        {
            IEnumerable<PublicationForm> publications = await _publicationService.GetPublications();
            if (publications == null) return NotFound();
            return Ok(publications);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublicationForm>> GetPublicationById(int id)
        {
            PublicationForm publication = await _publicationService.GetPublicationById(id);
            if (publication == null) return NotFound();
            return Ok(publication);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PublicationForm>> AddPublication([FromBody] PublicationForm publication)
        {
            PublicationForm _publication = await _publicationService.AddPublication(publication);
            if (_publication == null) return BadRequest();
            return Created($"api/publications/{_publication.Id}", _publication);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<PublicationForm>> ReplacePublication([FromBody] PublicationForm publication)
        {
            PublicationForm _publication = await _publicationService.ReplacePublication(publication);
            if (_publication == null) return NotFound();
            return Ok(_publication);
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePublicationById(int id)
        {
            bool success = await _publicationService.DeletePublicationById(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
