namespace StudentTeacherAPI.Models
{
    public class Record
    {
        public int RecordId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
