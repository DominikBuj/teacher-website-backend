using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TeacherWebsiteBackEnd.Data;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Controllers
{
    [Route("api/updates")]
    [ApiController]
    public class UpdatesController : Controller
    {
        private readonly IUpdateService _updateService;

        public UpdatesController(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Update>> GetUpdates()
        {
            IEnumerable<Update> updates = _updateService.GetUpdates();
            if (updates == null) return NotFound();
            return Ok(updates);
        }

        [HttpGet("{id}")]
        public ActionResult<Update> GetUpdateById(int id)
        {
            Update update = _updateService.GetUpdateById(id);
            if (update == null) return NotFound();
            return Ok(update);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Update> AddUpdate([FromBody] Update update)
        {
            if (update == null) return BadRequest();
            Update _update = _updateService.AddUpdate(update);
            return Created($"api/update/{_update.Id}", _update);
        }

        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<Update> ReplaceUpdate([FromBody] Update update)
        {
            if (update == null) return BadRequest();
            Update _update = _updateService.ReplaceUpdate(update);
            if (_update == null) return NotFound();
            return Ok(_update);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult<Update> DeleteUpdateById(int id)
        {
            bool success = _updateService.DeleteUpdateById(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
