namespace Chatbot_Web.Entities
{
    public class User
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
        public int UserTypeId { get; set; }
    }
}
