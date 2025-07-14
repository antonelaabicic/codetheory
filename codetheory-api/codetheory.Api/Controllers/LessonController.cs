using codetheory.BL.DTOs;
using codetheory.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace codetheory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "student, admin")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly ILessonContentService _contentService;

        public LessonController(ILessonService lessonService, ILessonContentService contentService)
        {
            _lessonService = lessonService;
            _contentService = contentService;
        }

        [HttpGet]
        public IActionResult GetAllLessons()
        {
            var lessons = _lessonService.GetAllLessons();
            return Ok(lessons);
        }

        [HttpGet("{lessonId}")]
        public IActionResult GetLesson(int lessonId)
        {
            var lesson = _lessonService.GetLessonById(lessonId);
            if (lesson == null)
            {
                return NotFound();
            }

            var contents = _contentService.GetContentsByLessonId(lessonId);
            lesson.Contents = contents.ToList();
            return Ok(lesson);
        }

        [HttpPost]
        public IActionResult AddLesson([FromBody] LessonDTO dto)
        {
            _lessonService.AddLesson(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLesson(int id, [FromBody] LessonDTO dto)
        {
            _lessonService.UpdateLesson(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLesson(int id)
        {
            _lessonService.DeleteLesson(id);
            return Ok();
        }
    }
}
