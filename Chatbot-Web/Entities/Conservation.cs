namespace Chatbot_Web.Entities
{
    public class Conservation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsClosed { get; set; }
        public int UserId { get; set; }
    }
}
