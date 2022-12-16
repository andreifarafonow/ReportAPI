using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace ReportAPI.Models.DTO
{
    public class UserDto
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        [SwaggerSchema(WriteOnly = true)]
        [JsonIgnore]
        public List<Report> Reports { get; set; } = new List<Report>();
    }
}
