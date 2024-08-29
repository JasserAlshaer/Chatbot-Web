namespace Chatbot_Web.DTOs.Messages
{
    public class FetchMessageDTO
    {
        public string Avatar {  get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}
