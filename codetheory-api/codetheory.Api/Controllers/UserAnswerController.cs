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
        private readonly IUserAnswerService _userAnswerService;
        private readonly IUserProgressService _userProgressService;
        public UserAnswerController(IUserAnswerService userAnswerService, IUserProgressService userProgressService)
        {
            _userAnswerService = userAnswerService;
            _userProgressService = userProgressService;
        }

        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<UserAnswerDto>> GetByUser(int userId)
        {
            var result = _userAnswerService.GetByUser(userId);
            return Ok(result);
        }

        [HttpGet("user/{userId}/answer/{answerId}")]
        public ActionResult<UserAnswerDto> GetByUserAndAnswer(int userId, int answerId)
        {
            var result = _userAnswerService.GetByUserAndAnswer(userId, answerId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("user/{userId}/lesson/{lessonId}")]
        public IActionResult SubmitAnswers(int userId, int lessonId, [FromBody] IEnumerable<UserAnswerDto> answers)
        {
            if (!answers.Any())
            {
                return BadRequest("No answers submitted.");
            }

            _userAnswerService.SubmitAnswers(answers);
            _userProgressService.EvaluateAndSaveProgress(userId, lessonId);

            return Ok();
        }

        [HttpPut("user/{userId}/lesson/{lessonId}")]
        public IActionResult UpdateAnswers(int userId, int lessonId, [FromBody] IEnumerable<UserAnswerDto> answers)
        {
            if (!answers.Any())
            {
                return BadRequest("No answers submitted.");
            }

            _userAnswerService.UpdateAnswers(answers);
            _userProgressService.EvaluateAndSaveProgress(userId, lessonId);

            return Ok();
        }

        [HttpGet("user/{userId}/lesson/{lessonId}")]
        public ActionResult<IEnumerable<UserAnswerDto>> GetByUserAndLesson(int userId, int lessonId)
        {
            var result = _userAnswerService.GetByUserAndLesson(userId, lessonId);
            return Ok(result);
        }

        [HttpGet("user/{userId}/lesson/{lessonId}/progress")]
        public ActionResult<UserProgressDto> GetProgress(int userId, int lessonId)
        {
            var progress = _userProgressService.GetProgress(userId, lessonId);
            if (progress == null) return NotFound();
            return Ok(progress);
        }

    }
}
