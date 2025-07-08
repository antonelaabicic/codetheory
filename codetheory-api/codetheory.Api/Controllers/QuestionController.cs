using codetheory.BL.Services.Impl;
using codetheory.BL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace codetheory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;

        public QuestionController(IQuestionService questionService, IAnswerService answerService)
        {
            _questionService = questionService;
            _answerService = answerService;
        }

        [HttpGet]
        public IActionResult GetAllQuestions()
        {
            var lessons = _questionService.GetAllQuestions();
            return Ok(lessons);
        }

        [HttpGet("{questionId}")]
        public IActionResult GetQuestion(int questionId)
        {
            var question = _questionService.GetQuestionById(questionId);
            if (question == null)
            {
                return NotFound();
            }

            var answers = _answerService.GetAnswersByQuestionId(questionId);
            question.Answers = answers.ToList();
            return Ok(question);
        }

        [HttpGet("{lessonId}/quiz")]
        public IActionResult GetQuestionsPerLesson(int lessonId)
        {
            var questions = _questionService.GetQuestionsByLessonId(lessonId);
            foreach (var question in questions)
            {
                var answers = _answerService.GetAnswersByQuestionId(question.Id);
                question.Answers = answers.ToList();
            }
            return Ok(questions);
        }
    }
}
