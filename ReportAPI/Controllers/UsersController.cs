using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportAPI.Models;
using ReportAPI.Models.DTO;

namespace ReportAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ReportApiDbContext _db;
        private readonly IMapper _mapper;
        private readonly IValidator<UserDto> _validator;

        public UsersController(ReportApiDbContext db, IMapper mapper, IValidator<UserDto> validator)
        {
            _db = db;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"Пользователь с идентификатором {id} не найден"
                });
            }

            return _mapper.Map<UserDto>(user);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            List<User> users = await _db.Users.ToListAsync();

            List<UserDto> result = _mapper.Map<List<UserDto>>(users);

            return result;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDto user)
        {
            if (id != user.Id)
            {
                return BadRequest(new ErrorDto()
                {
                    Message = "Идентификатор пользователя в запросе не совпадает с идентификатором в теле запроса."
                });
            }

            ValidationResult validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorDto()
                {
                    Message = validationResult.Errors.First().ErrorMessage
                });
            }

            if (_db.Users.Any(u => u.Id != id && u.Email == user.Email))
                return Conflict(new ErrorDto()
                {
                    Message = $"Пользователь с email {user.Email} уже существует"
                });

            _db.Entry(_mapper.Map<User>(user)).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.Users.Any(x => x.Id == id))
                {
                    return NotFound(new ErrorDto()
                    {
                        Message = $"Пользователь с идентификатором {id} не найден"
                    });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> PutUsers(IEnumerable<UserDto> users)
        {
            foreach (var user in users)
            {
                ValidationResult validationResult = await _validator.ValidateAsync(user);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new ErrorDto()
                    {
                        Message = validationResult.Errors.First().ErrorMessage
                    });
                }

                if (!_db.Users.Any(x => x.Id == user.Id))
                {
                    return NotFound(new ErrorDto() 
                    {
                        Message = $"Не удалось выполнить обновление данных пользователя с идентификатором {user.Id}. " +
                        $"Указанный пользователь не существует."
                    });
                }

                if (await _db.Users.AnyAsync(u => u.Id != user.Id && u.Email == user.Email))
                    return Conflict(new ErrorDto()
                    {
                        Message = $"Пользователь с email {user.Email} уже существует"
                    });

                _db.Entry(_mapper.Map<User>(user)).State = EntityState.Modified;
            }


            await _db.SaveChangesAsync();


            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(UserDto user)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorDto()
                {
                    Message = validationResult.Errors.First().ErrorMessage
                });
            }

            if (await _db.Users.AnyAsync(u => u.Email == user.Email))
            {
                return Conflict(new ErrorDto()
                {
                    Message = $"Пользователь с email {user.Email} уже существует"
                });
            }

            var trackingUser = _db.Users.Add(_mapper.Map<User>(user)).Entity;

            await _db.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = trackingUser.Id }, _mapper.Map<UserDto>(trackingUser));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new ErrorDto()
                {
                    Message = $"Пользователь с идентификатором {id} не найден"
                });
            }

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteUsers()
        {
            foreach (var user in _db.Users)
            {
                _db.Users.Remove(user);
            }

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
