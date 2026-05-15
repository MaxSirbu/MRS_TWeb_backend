using Microsoft.AspNetCore.Mvc;
using Training_and_Workout_App.Domain.Entities;
using Training_and_Workout_App.Domain.Models;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimerController : ControllerBase
{
    private static readonly List<TimerSession> _timerSessions = [];

    /// <summary>
    /// Create a new timer session
    /// </summary>
    [HttpPost]
    public IActionResult CreateTimerSession([FromBody] CreateTimerSessionDto dto)
    {
        if (dto.DurationSeconds <= 0)
        {
            return BadRequest(new { error = "Duration must be greater than 0" });
        }

        // In production, this would come from authentication context
        var userId = 1; // Mock user ID

        var session = new TimerSession
        {
            UserId = userId,
            DurationSeconds = dto.DurationSeconds,
            ExerciseName = dto.ExerciseName,
            Notes = dto.Notes,
            StartedAt = DateTime.UtcNow,
            Status = TimerStatus.Running,
            CreatedAt = DateTime.UtcNow
        };

        // Mock storage - in production, use DbContext
        _timerSessions.Add(session);

        var response = MapToResponseDto(session);
        return CreatedAtAction(nameof(GetTimerSession), new { id = session.Id }, response);
    }

    /// <summary>
    /// Get a specific timer session by ID
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetTimerSession(int id)
    {
        var session = _timerSessions.FirstOrDefault(s => s.Id == id);
        if (session == null)
        {
            return NotFound(new { error = "Timer session not found" });
        }

        return Ok(MapToResponseDto(session));
    }

    /// <summary>
    /// Get all timer sessions for the current user
    /// </summary>
    [HttpGet]
    public IActionResult GetTimerSessions([FromQuery] int? limit = null, [FromQuery] int? offset = 0)
    {
        // In production, filter by authenticated user
        var userId = 1;

        var query = _timerSessions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .AsEnumerable();

        if (offset.HasValue && offset > 0)
        {
            query = query.Skip(offset.Value);
        }

        if (limit.HasValue)
        {
            query = query.Take(limit.Value);
        }

        var sessions = query.Select(MapToResponseDto).ToList();
        return Ok(new
        {
            total = _timerSessions.Count(s => s.UserId == userId),
            data = sessions
        });
    }

    /// <summary>
    /// Update a timer session (complete, pause, or add notes)
    /// </summary>
    [HttpPut("{id}")]
    public IActionResult UpdateTimerSession(int id, [FromBody] UpdateTimerSessionDto dto)
    {
        var session = _timerSessions.FirstOrDefault(s => s.Id == id);
        if (session == null)
        {
            return NotFound(new { error = "Timer session not found" });
        }

        if (!string.IsNullOrEmpty(dto.Status))
        {
            if (Enum.TryParse<TimerStatus>(dto.Status, true, out var status))
            {
                session.Status = status;
                if (status == TimerStatus.Completed)
                {
                    session.CompletedAt = DateTime.UtcNow;
                }
            }
            else
            {
                return BadRequest(new { error = "Invalid status value" });
            }
        }

        if (!string.IsNullOrEmpty(dto.Notes))
        {
            session.Notes = dto.Notes;
        }

        return Ok(MapToResponseDto(session));
    }

    /// <summary>
    /// Get timer statistics for the current user
    /// </summary>
    [HttpGet("statistics/overview")]
    public IActionResult GetTimerStatistics()
    {
        var userId = 1;
        var sessions = _timerSessions.Where(s => s.UserId == userId).ToList();

        var completed = sessions.Where(s => s.Status == TimerStatus.Completed).ToList();
        var totalSeconds = completed.Sum(s => s.DurationSeconds);
        var avgSeconds = completed.Count > 0 ? totalSeconds / completed.Count : 0;

        var stats = new TimerSessionStatisticsDto
        {
            TotalSessions = sessions.Count,
            TotalSeconds = totalSeconds,
            CompletedSessions = completed.Count,
            AverageSessionSeconds = avgSeconds,
            MostUsedExercise = sessions
                .Where(s => !string.IsNullOrEmpty(s.ExerciseName))
                .GroupBy(s => s.ExerciseName)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key
        };

        return Ok(stats);
    }

    /// <summary>
    /// Delete a timer session
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult DeleteTimerSession(int id)
    {
        var session = _timerSessions.FirstOrDefault(s => s.Id == id);
        if (session == null)
        {
            return NotFound(new { error = "Timer session not found" });
        }

        _timerSessions.Remove(session);
        return NoContent();
    }

    private static TimerSessionResponseDto MapToResponseDto(TimerSession session)
    {
        return new TimerSessionResponseDto
        {
            Id = session.Id,
            UserId = session.UserId,
            DurationSeconds = session.DurationSeconds,
            StartedAt = session.StartedAt,
            CompletedAt = session.CompletedAt,
            Status = session.Status.ToString(),
            ExerciseName = session.ExerciseName,
            Notes = session.Notes,
            CreatedAt = session.CreatedAt
        };
    }
}
