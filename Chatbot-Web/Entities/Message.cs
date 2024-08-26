namespace Chatbot_Web.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsResponseFromChatbot { get; set; }
        public int ConservationId { get; set; }
    }
}
