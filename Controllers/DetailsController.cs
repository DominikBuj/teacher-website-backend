using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Data;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;

namespace TeacherWebsiteBackEnd.Controllers
{
    [Route("api/details")]
    [ApiController]
    public class DetailsController : Controller
    {
        private readonly ITextService _textService;

        public DetailsController(ITextService textService)
        {
            _textService = textService;
        }

        [HttpGet]
        public async Task<ActionResult<Details>> GetDetails()
        {
            Details details = new Details();

            foreach (PropertyInfo propertyInfo in typeof(Details).GetProperties())
            {
                Text text = await _textService.GetTextByName(propertyInfo.Name);
                propertyInfo.SetValue(details, (text != null) ? text.Value : null);
            }

            return Ok(details);
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult<Details>> UpdateDetails([FromBody] JsonPatchDocument<Details> update)
        {
            Details details = (await GetDetails()).Value;
            Details _details = (await GetDetails()).Value;
            if (details == null || _details == null)
            {
                details = new Details();
                _details = new Details();
            }

            update.ApplyTo(_details, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            foreach (PropertyInfo propertyInfo in typeof(Details).GetProperties())
            {
                string originalValue = (string)propertyInfo.GetValue(details);
                string updatedValue = (string)propertyInfo.GetValue(_details);
                if (originalValue != updatedValue) await _textService.ReplaceText(new Text { Name = propertyInfo.Name, Value = updatedValue });
            }

            return Ok(_details);
        }
    }
}
