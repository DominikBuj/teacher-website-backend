using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Data;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Controllers
{
    [Route("api/texts")]
    [ApiController]
    public class TextsController : Controller
    {
        private readonly ITextService _textService;

        public TextsController(ITextService textService)
        {
            _textService = textService;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Text>> GetTextByName(string name)
        {
            Text text = await _textService.GetTextByName(name);
            if (text == null) return NotFound();
            return Ok(text);
        }

        [Authorize]
        [HttpPut("name")]
        public async Task<ActionResult<Text>> ReplaceText([FromBody] Text text)
        {
            Text _text = await _textService.ReplaceText(text);
            if (_text == null) return BadRequest();
            return Ok(_text);
        }
    }
}
