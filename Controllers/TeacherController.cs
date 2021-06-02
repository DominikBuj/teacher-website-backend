using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Data;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;

namespace TeacherWebsiteBackEnd.Controllers
{
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : Controller
    {
        private readonly ITextService _textService;

        public TeacherController(ITextService textService)
        {
            _textService = textService;
        }

        [HttpGet]
        public async Task<ActionResult<Teacher>> GetTeacher()
        {
            Teacher teacher = new Teacher();

            foreach (PropertyInfo propertyInfo in typeof(Teacher).GetProperties())
            {
                Text text = await _textService.GetTextByName(propertyInfo.Name);
                propertyInfo.SetValue(teacher, (text != null) ? text.Value : null);
            }

            return Ok(teacher);
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult<Teacher>> UpdateTeacher([FromBody] JsonPatchDocument<Teacher> update)
        {
            Teacher teacher = (await GetTeacher()).Value;
            Teacher _teacher = (await GetTeacher()).Value;
            if (teacher == null || _teacher == null)
            {
                teacher = new Teacher();
                _teacher = new Teacher();
            }

            update.ApplyTo(_teacher, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            foreach (PropertyInfo propertyInfo in typeof(Teacher).GetProperties())
            {
                string originalValue = (string) propertyInfo.GetValue(teacher);
                string updatedValue = (string) propertyInfo.GetValue(_teacher);
                if (originalValue != updatedValue) await _textService.ReplaceText(new Text { Name = propertyInfo.Name, Value = updatedValue });
            }

            return Ok(_teacher);
        }
    }
}
