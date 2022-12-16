namespace ReportAPI.Models
{
    public class Report
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public int HoursCount;

        public DateTime Date { get; set; }

        public User User { get; set; }
    }
}
