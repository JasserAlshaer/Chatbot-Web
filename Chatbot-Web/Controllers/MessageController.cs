using Chatbot_Web.Context;
using Chatbot_Web.DTOs.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chatbot_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ChatbotContext _context;
        public MessageController(ChatbotContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetMessageListByConservationId(int Id)
        {
            var query = from conservation in _context.Conservations
                        join message in _context.Messages
                        on conservation.Id equals message.ConservationId
                        join user in _context.Users
                        on conservation.UserId equals user.Id
                        where conservation.Id == Id
                        select new FetchMessageDTO
                        {
                            Author = message.IsResponseFromChatbot?"Mira": user.FirstName + "-" + user.LastName,
                            Avatar = message.IsResponseFromChatbot ? "https://img.freepik.com/free-vector/chatbot-chat-message-vectorart_78370-4104.jpg?size=338&ext=jpg&ga=GA1.1.2008272138.1724803200&semt=ais_hybrid"
                            : "https://cdn-icons-png.flaticon.com/512/3607/3607444.png",
                            IsCurrentUser = message.IsResponseFromChatbot,
                            Text = message.Text,
                            Time = message.CreationDate.ToShortTimeString()
                        };
            return Ok(await query.ToListAsync());
        } 
    }
}
