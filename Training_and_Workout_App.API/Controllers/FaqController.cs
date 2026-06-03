using Training_and_Workout_App.BusinessLayer.Interfaces;
using Training_and_Workout_App.Domain.Models.FAQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/faq")]
[Authorize]
public class FaqController(IFaqAction faqActions) : AppControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
        => Ok(await faqActions.GetAllAsync());

    [HttpPost("categories")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCategory([FromBody] FaqCategoryUpsertDto dto)
        => Ok(await faqActions.CreateCategoryAsync(dto));

    [HttpPut("categories/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] FaqCategoryUpsertDto dto)
    {
        var category = await faqActions.UpdateCategoryAsync(id, dto);
        if (category is null) return NotFound();
        return Ok(category);
    }

    [HttpDelete("categories/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await faqActions.DeleteCategoryAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    [HttpPost("questions")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateQuestion([FromBody] FaqQuestionUpsertDto dto)
    {
        var question = await faqActions.CreateQuestionAsync(dto);
        if (question is null) return NotFound();
        return Ok(question);
    }

    [HttpPut("questions/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateQuestion(int id, [FromBody] FaqQuestionUpsertDto dto)
    {
        var question = await faqActions.UpdateQuestionAsync(id, dto);
        if (question is null) return NotFound();
        return Ok(question);
    }

    [HttpDelete("questions/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var deleted = await faqActions.DeleteQuestionAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
