using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
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
        public ActionResult<Teacher> GetTeacher()
        {
            Teacher teacher = new Teacher();

            foreach (PropertyInfo propertyInfo in typeof(Teacher).GetProperties())
            {
                Text text = _textService.GetTextByName(propertyInfo.Name);
                if (text == null) _textService.AddText(new Text(propertyInfo.Name, ""));
                propertyInfo.SetValue(teacher, (text != null) ? text.Value : null);
            }

            return teacher;
        }

        [Authorize]
        [HttpPatch]
        public ActionResult<Teacher> UpdateTeacher([FromBody] JsonPatchDocument<Teacher> update)
        {
            if (update == null) return BadRequest();

            Teacher _teacher = GetTeacher().Value;
            Teacher __teacher = GetTeacher().Value;
            update.ApplyTo(__teacher, ModelState);

            if (!ModelState.IsValid) return BadRequest();

            foreach (PropertyInfo propertyInfo in typeof(Teacher).GetProperties())
            {
                string originalValue = (string)propertyInfo.GetValue(_teacher);
                string updatedValue = (string)propertyInfo.GetValue(__teacher);
                if (originalValue != updatedValue) _textService.AddText(new Text(propertyInfo.Name, updatedValue));
            }

            return Ok(__teacher);
        }
    }
}
