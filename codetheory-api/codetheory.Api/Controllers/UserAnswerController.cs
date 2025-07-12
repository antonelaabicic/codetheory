using codetheory.BL.DTOs;
using codetheory.BL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace codetheory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAnswerController : ControllerBase
    {
        private readonly IUserAnswerService _service;
        public UserAnswerController(IUserAnswerService service)
        {
            _service = service;
        }

        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<UserAnswerDto>> GetByUser(int userId)
        {
            var result = _service.GetByUser(userId);
            return Ok(result);
        }

        [HttpGet("user/{userId}/answer/{answerId}")]
        public ActionResult<UserAnswerDto> GetByUserAndAnswer(int userId, int answerId)
        {
            var result = _service.GetByUserAndAnswer(userId, answerId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult SubmitAnswers([FromBody] IEnumerable<UserAnswerDto> answers)
        {
            _service.SubmitAnswers(answers);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateAnswers([FromBody] IEnumerable<UserAnswerDto> answers)
        {
            _service.UpdateAnswers(answers);
            return Ok();
        }

        [HttpGet("user/{userId}/lesson/{lessonId}")]
        public ActionResult<IEnumerable<UserAnswerDto>> GetByUserAndLesson(int userId, int lessonId)
        {
            var result = _service.GetByUserAndLesson(userId, lessonId);
            return Ok(result);
        }
    }
}
