using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportAPI.Models;
using ReportAPI.Models.DTO;

namespace ReportAPI.Controllers
{
    [Route("api/users/{userId}/reports/")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ReportApiDbContext _db;
        private readonly IMapper _mapper;
        private readonly IValidator<ReportDto> _validator;

        public ReportsController(ReportApiDbContext db, IMapper mapper, IValidator<ReportDto> validator)
        {
            _db = db;
            _mapper = mapper;
            _validator = validator;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetReports([FromRoute] int userId, [FromQuery] DateTime? month = null)
        {
            var user = await _db.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"Пользователь с идентификатором {userId} не найден"
                });
            }

            List<Report> reports;
            
            if(month == null)
                reports = await _db.Reports.Where(x => x.User.Id == userId).ToListAsync();
            else
                reports = await _db.Reports
                    .Where(x => x.User.Id == userId)
                    .Where(x => x.Date.Year == month.Value.Year && x.Date.Month == month.Value.Month)
                    .ToListAsync();

            List<ReportDto> result = _mapper.Map<List<ReportDto>>(reports);

            return result;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ReportDto>> GetReport([FromRoute] int userId, int id)
        {
            var user = await _db.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"Пользователь с идентификатором {userId} не найден"
                });
            }

            var report = await _db.Reports.Where(x => x.User.Id == userId).FirstOrDefaultAsync(x => x.Id == id);

            if (report == null)
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"У пользователя {userId} отсутствует отчёт с идентификатором {id}"
                });
            }

            return _mapper.Map<ReportDto>(report);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutReport([FromRoute] int userId, int id, ReportDto report)
        {
            var user = await _db.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"Пользователь с идентификатором {userId} не найден"
                });
            }

            ValidationResult validationResult = await _validator.ValidateAsync(report);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorDto()
                {
                    Message = validationResult.Errors.First().ErrorMessage
                });
            }

            if (id != report.Id)
            {
                return BadRequest(new ErrorDto()
                {
                    Message = "Идентификатор отчёта в запросе не совпадает с идентификатором в теле запроса."
                });
            }

            _db.Entry(_mapper.Map<Report>(report)).State = EntityState.Modified;


            if (!_db.Reports.Any(x => x.Id == report.Id && x.User.Id == userId))
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"Не удалось выполнить обновление отчётов пользователя с идентификатором {userId}. " +
                    $"У пользователя отсутствует отчёт с идентификатором {report.Id}."
                });
            }


            await _db.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> PutReports([FromRoute] int userId, int id, IEnumerable<ReportDto> reports)
        {
            var user = await _db.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"Пользователь с идентификатором {userId} не найден"
                });
            }


            foreach(var report in reports)
            {
                ValidationResult validationResult = await _validator.ValidateAsync(report);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new ErrorDto()
                    {
                        Message = validationResult.Errors.First().ErrorMessage
                    });
                }

                if (!_db.Reports.Any(x => x.Id == report.Id && x.User.Id == userId))
                {
                    return NotFound(new ErrorDto()
                    {
                        Message = $"Не удалось выполнить обновление отчётов пользователя с идентификатором {userId}. " +
                        $"У пользователя отсутствует отчёт с идентификатором {report.Id}."
                    });
                }

                _db.Entry(_mapper.Map<Report>(report)).State = EntityState.Modified;
            }


            await _db.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<ReportDto>> PostReport([FromRoute] int userId, ReportDto report)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(report);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorDto()
                {
                    Message = validationResult.Errors.First().ErrorMessage
                });
            }

            User targetUser = await _db.Users.FindAsync(userId);

            if (targetUser == null)
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"Пользователь с идентификатором {userId} не найден"
                });
            }

            var result = _mapper.Map<Report>(report);

            targetUser.Reports.Add(result);
            await _db.SaveChangesAsync();


            return CreatedAtAction("GetReport", new { id = report.Id, userId = userId }, _mapper.Map<ReportDto>(result));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport([FromRoute] int userId, int id)
        {
            User targetUser = await _db.Users.FindAsync(userId);

            if (targetUser == null)
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"Пользователь с идентификатором {userId} не найден"
                });
            }

            var report = await _db.Reports.Where(x => x.User.Id == userId).FirstOrDefaultAsync(x => x.Id == id);

            if (report == null)
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"У пользователя {userId} отсутствует отчёт с идентификатором {id}"
                });
            }

            _db.Reports.Remove(report);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReports([FromRoute] int userId)
        {
            User targetUser = await _db.Users.FindAsync(userId);

            if (targetUser == null)
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"Пользователь с идентификатором {userId} не найден"
                });
            }


            foreach (var report in _db.Reports.Where(x => x.User.Id == userId))
            {
                _db.Reports.Remove(report);
            }

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
