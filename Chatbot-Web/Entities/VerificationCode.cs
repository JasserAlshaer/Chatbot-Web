namespace Chatbot_Web.Entities
{
    public class VerificationCode
    {
        public int Id { get; set; } 
        public string Email { get; set; }
        public string OTP { get; set; }
        public DateTime ExpireDate  { get; set; }
    }
}
