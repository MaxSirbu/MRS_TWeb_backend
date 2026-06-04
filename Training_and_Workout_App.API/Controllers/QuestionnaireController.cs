using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models.Questionnaire;
using Training_and_Workout_App.Domain.Models.QuestionnaireEntry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/questionnaire")]
[Authorize]
public class QuestionnaireController(IQuestionnaireAction questionnaireActions) : AppControllerBase
{
    // GET api/questionnaire/questions — intrebari publice ale chestionarului
    [HttpGet("questions")]
    [AllowAnonymous]
    public async Task<IActionResult> GetQuestions()
        => Ok(await questionnaireActions.GetAllQuestionsAsync());

    // GET api/questionnaire/entries?userId=1
    [HttpGet("entries")]
    public async Task<IActionResult> GetEntries([FromQuery] int userId)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await questionnaireActions.GetEntriesByUserAsync(userId)));
    }

    // POST api/questionnaire/submit?userId=1
    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromQuery] int userId, [FromBody] QuestionnaireEntryCreateDto dto)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await questionnaireActions.SubmitAnswerAsync(userId, dto)));
    }

    // POST api/questionnaire/complete?userId=1
    [HttpPost("complete")]
    public async Task<IActionResult> Complete([FromQuery] int userId, [FromBody] QuestionnaireCompleteDto dto)
    {
        return await ForOwnedUserAsync(
            userId,
            async () => Ok(await questionnaireActions.CompleteAsync(userId, dto)));
    }
}
