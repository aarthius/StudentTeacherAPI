namespace StudentTeacherAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Designation {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
