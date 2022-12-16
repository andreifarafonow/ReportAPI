using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ReportAPI.Models.DTO
{
    public class ReportDto
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        public string Comment { get; set; }

        public int? HoursCount { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
