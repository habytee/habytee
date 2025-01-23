using Microsoft.AspNetCore.Mvc;
using habytee.Interconnection.Models;
using habytee.Server.DataAccess;
using habytee.Interconnection.Models.Requests;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace habytee.Server.Controllers
{
	[ApiController]
	[Route("api/habit")]
	public class HabitController : ControllerBase
	{
		private readonly WriteDbContext writeDbContext;
		private readonly ReadDbContext readDbContext;
		private readonly DataService dataService;

		public HabitController(WriteDbContext writeDbContext, ReadDbContext readDbContext, DataService dataService)
		{
			this.writeDbContext = writeDbContext;
			this.readDbContext = readDbContext;
			this.dataService = dataService;
		}

		[HttpPost]
		public IActionResult CreateHabit([FromBody] CreateHabitRequest habitRequest)
		{
			if (habitRequest == null)
			{
				return BadRequest("Habit data is required.");
			}

			var email = Request.Headers["X-Forwarded-Email"].FirstOrDefault();
			if (email == null)
			{
				return BadRequest("OAuth2 proxy error");
			}

			var user = dataService.GetReadUser(email);

            if(habitRequest.Alarm != null)
            {
                habitRequest.Alarm = DateTime.SpecifyKind(habitRequest.Alarm.Value, DateTimeKind.Utc);
            }
			var habit = new Habit
			{
				Name = habitRequest.Name,
				Reason = habitRequest.Reason,
				ABBoth = habitRequest.ABBoth,
				AWeekDays = habitRequest.AWeekDays,
				BWeekDays = habitRequest.BWeekDays,
				Alarm = habitRequest.Alarm,
				Earnings = habitRequest.Earnings,
				UserId = user.Id
			};

			writeDbContext.Habits.Add(habit);
			writeDbContext.SaveChanges();

			return Ok(new { message = "Habit created successfully", id = habit.Id });
		}

		[HttpGet("day/{day}")]
		public IActionResult GetAllHabitsByDay(int day)
		{
			List<Habit> habitList = [];
			var email = Request.Headers["X-Forwarded-Email"].FirstOrDefault();
			if (email == null)
			{
				return BadRequest("OAuth2 proxy error");
			}
			if (day < -1 || day > 1) 
			{
				return BadRequest("parameter error");
			}

			var user = dataService.GetReadUser(email);
			var targetDate = DateTime.UtcNow.AddDays(day);
			var habits = readDbContext.Habits.Where(h => h.UserId == user.Id).Include(h => h.HabitCheckedEvents).ToList();

			var calendar = CultureInfo.CurrentCulture.Calendar;
			var weekOfYear = calendar.GetWeekOfYear(targetDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
			bool isAWeek = weekOfYear % 2 != 0;

			var weekday = targetDate.DayOfWeek;
			foreach (var habit in habits)
			{
				if (!habit.ABBoth)
				{
					habit.BWeekDays = habit.AWeekDays;
				}
				var activeWeekDays = isAWeek ? habit.AWeekDays : habit.BWeekDays;
				
				if (activeWeekDays.Contains(weekday))
				{
					if (day >= 0 || (habit.CreationDate?.Date <= targetDate.Date))
					{
						habitList.Add(habit);
					}
				}
			}
			return Ok(habitList);
		}

		[HttpGet("id/{id}")]
		public IActionResult GetHabitById(int id)
		{
			var habit = readDbContext.Habits.FirstOrDefault(h => h.Id == id);
			if (habit == null)
			{
				return NotFound("Habit not found.");
			}

			return Ok(habit);
		}

		[HttpPut("{id}")]
		public IActionResult UpdateHabit(int id, [FromBody] UpdateHabitRequest habitRequest)
		{
			if (habitRequest == null)
			{
				return BadRequest("Habit data is required.");
			}

			var habit = writeDbContext.Habits.FirstOrDefault(h => h.Id == id);
			if (habit == null)
			{
				return NotFound("Habit not found.");
			}

			habit.Name = habitRequest.Name;
			habit.Reason = habitRequest.Reason;
			habit.ABBoth = habitRequest.ABBoth;
			habit.AWeekDays = habitRequest.AWeekDays;
			habit.BWeekDays = habitRequest.BWeekDays;
			habit.Alarm = habitRequest.Alarm;
			habit.Earnings = habitRequest.Earnings;

			writeDbContext.SaveChanges();

			return Ok(new { message = "Habit updated successfully" });
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteHabit(int id)
		{
			var habit = writeDbContext.Habits.FirstOrDefault(h => h.Id == id);
			if (habit == null)
			{
				return NotFound("Habit not found.");
			}

			writeDbContext.Habits.Remove(habit);
			writeDbContext.SaveChanges();

			return Ok(new { message = "Habit deleted successfully" });
		}

		[HttpPost("{id}")]
		public IActionResult ToggleCheckmark(int id)
		{
			var habit = writeDbContext.Habits.FirstOrDefault(h => h.Id == id);
			if (habit == null)
			{
				return NotFound("Habit not found.");
			}

			var existingEvent = writeDbContext.HabitCheckedEvents.FirstOrDefault(e => e.HabitId == habit.Id);

			if (existingEvent != null)
			{
				writeDbContext.HabitCheckedEvents.Remove(existingEvent);
				writeDbContext.SaveChanges();
				return Ok("Habit Unchecked");
			}
			else
			{
				var newEvent = new HabitCheckedEvent
				{
					HabitId = habit.Id,
					Habit = habit,
					TimeStamp = DateTime.UtcNow
				};

				writeDbContext.HabitCheckedEvents.Add(newEvent);
				writeDbContext.SaveChanges();
				return Ok("HabitCheckedEvent added.");
			}
		}

		[HttpGet("stats")]
		public IActionResult GetStatistics()
		{
			var email = Request.Headers["X-Forwarded-Email"].FirstOrDefault();
			if (email == null)
			{
				return BadRequest("OAuth2 proxy error");
			}

			var user = dataService.GetReadUser(email);
			var endDate = DateTime.UtcNow;
			var startDate = endDate.AddDays(-14);

			var habits = readDbContext.Habits
				.Where(h => h.UserId == user.Id && h.CreationDate <= endDate)
				.Include(h => h.HabitCheckedEvents)
				.ToList();

			// Filter HabitCheckedEvents to only include events within our date range
			foreach (var habit in habits)
			{
				habit.HabitCheckedEvents = habit.HabitCheckedEvents
					.Where(e => e.TimeStamp >= startDate && e.TimeStamp <= endDate)
					.ToList();
			}

			return Ok(habits);
		}
	}
}
