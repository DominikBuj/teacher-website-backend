using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public ActionResult<IEnumerable<Publication>> GetPublications()
        {
            IEnumerable<Publication> publications = _publicationService.GetPublications();
            if (publications == null) return NotFound();
            return Ok(publications);
        }

        [HttpGet("{id}")]
        public ActionResult<Publication> GetPublicationById(int id)
        {
            Publication publication = _publicationService.GetPublicationById(id);
            if (publication == null) return NotFound();
            return Ok(publication);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Publication> AddPublication([FromBody] Publication publication)
        {
            if (publication == null) return BadRequest();
            Publication _publication = _publicationService.AddPublication(publication);
            return Created($"api/publication/{_publication.Id}", _publication);
        }

        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<Publication> ReplacePublication([FromBody] Publication publication)
        {
            if (publication == null) return BadRequest();
            Publication _publication = _publicationService.ReplacePublication(publication);
            if (_publication == null) return NotFound();
            return Ok(_publication);
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult<Publication> DeletePublicationById(int id)
        {
            bool success = _publicationService.DeletePublicationById(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
