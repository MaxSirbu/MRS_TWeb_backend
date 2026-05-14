using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionnaireController(IQuestionnaireService questionnaireService) : ControllerBase
{
    // GET api/questionnaire/questions
    [HttpGet("questions")]
    public async Task<IActionResult> GetQuestions()
        => Ok(await questionnaireService.GetAllQuestionsAsync());

    // GET api/questionnaire/entries?userId=1
    [HttpGet("entries")]
    public async Task<IActionResult> GetEntries([FromQuery] int userId)
        => Ok(await questionnaireService.GetEntriesByUserAsync(userId));

    // POST api/questionnaire/submit?userId=1
    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromQuery] int userId, [FromBody] QuestionnaireEntryCreateDto dto)
        => Ok(await questionnaireService.SubmitAnswerAsync(userId, dto));
}
