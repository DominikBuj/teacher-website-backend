using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Data;
using TeacherWebsiteBackEnd.Entities;

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
        public async Task<ActionResult<IEnumerable<Publication>>> GetPublications()
        {
            IEnumerable<Publication> publications = await _publicationService.GetPublications();
            if (publications == null) return NotFound();
            return Ok(publications);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Publication>> GetPublicationById(int id)
        {
            Publication publication = await _publicationService.GetPublicationById(id);
            if (publication == null) return NotFound();
            return Ok(publication);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Publication>> AddPublication([FromBody] Publication publication)
        {
            Publication _publication = await _publicationService.AddPublication(publication);
            if (_publication == null) return BadRequest();
            return Created($"api/publications/{_publication.Id}", _publication);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Publication>> ReplacePublication([FromBody] Publication publication)
        {
            Publication _publication = await _publicationService.ReplacePublication(publication);
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
