using Chatbot_Web.Context;
using Chatbot_Web.DTOs.Messages;
using Chatbot_Web.Entities;
using Chatbot_Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Chatbot_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly ChatbotContext _context;
        public ChatbotController(ChatbotContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetWelcomeMessage([FromQuery] int userId, [FromQuery] string title)
        {
            Conservation conservation = new Conservation()
            {
                CreatedDate = DateTime.Now,
                UserId = userId,
                Title = title,
                IsClosed = false
            };
            _context.Add(conservation);
            _context.SaveChanges();
            string message = WelcomeMessageGeneratorHelper.GetWelcomeMessage();
            return Ok(message);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ClosedConservation([FromQuery] int Id)
        {
            var conservattion = _context.Conservations.Where(x => x.Id == Id
            && x.IsClosed == false).FirstOrDefault();
            if (conservattion != null)
            {
                conservattion.IsClosed = true;
                conservattion.EndDate = DateTime.Now;
                _context.Update(conservattion);
                _context.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult ExecuteCommand([FromBody] SendMessageDTO input)
        {
            string? commandResult = null;
            Message message = new Message()
            {
                ConservationId = input.ConservationId,
                Text = input.Text,
                CreationDate = DateTime.Now,
                IsResponseFromChatbot = false
            };
            _context.Add(message);
            _context.SaveChanges();
            if (input.Text.Contains("ping", StringComparison.OrdinalIgnoreCase))
            {
                string website = ExtractWebsite(input.Text);
                if (!string.IsNullOrEmpty(website))
                {
                    commandResult = ExecutePing(website);
                    CreationMessage(commandResult, input.ConservationId);
                    return Ok(commandResult);
                }
                else
                {
                    commandResult = "No valid website provided to ping.";
                    CreationMessage(commandResult, input.ConservationId);
                    return BadRequest(commandResult);
                }
            }
            else if (input.Text.Contains("trace route", StringComparison.OrdinalIgnoreCase))
            {
                string website = ExtractWebsite(input.Text);
                if (!string.IsNullOrEmpty(website))
                {
                    commandResult = ExecuteTraceroute(website);
                    CreationMessage(commandResult, input.ConservationId);
                    return Ok(commandResult);
                }
                else
                {
                    commandResult = "No valid website provided to trace.";
                    CreationMessage(commandResult, input.ConservationId);
                    return BadRequest(commandResult);
                }
            }
            else
            {
                commandResult = "Command not recognized.";
                CreationMessage(commandResult, input.ConservationId);
                return BadRequest(commandResult);
            }


        }

        [HttpDelete]
        [Route("[action]")]
        public IActionResult DeleteConservation([FromQuery] int Id)
        {
            var conservattion = _context.Conservations.Where(x => x.Id == Id
            && x.IsClosed == true).FirstOrDefault();
            if (conservattion != null)
            {
                var messages = _context.Messages.Where(x => x.ConservationId == Id).ToList();
                if (messages.Count > 0)
                {
                    foreach (var message in messages)
                    {
                        _context.Remove(message);
                        _context.SaveChanges();
                    }
                }
                _context.Remove(conservattion);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        #region ExractingMethod
        private string ExtractWebsite(string message)
        {
            // Simple extraction based on spaces, more complex logic can be added
            string[] words = message.Split(' ');
            foreach (var word in words)
            {
                if (Uri.IsWellFormedUriString(word, UriKind.Absolute))
                {
                    return word;
                }
                else if (word.Contains("."))
                {
                    return word; // Assuming it's a domain
                }
            }
            return null;
        }

        private void CreationMessage(string result, int conservationId)
        {
            Message message = new Message()
            {
                ConservationId = conservationId,
                Text = result,
                CreationDate = DateTime.Now,
                IsResponseFromChatbot = true
            };
            _context.Add(message);
            _context.SaveChanges();
        }
        private string ExecutePing(string host)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "ping";
                process.StartInfo.Arguments = host;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (string.IsNullOrEmpty(error))
                {
                    return output;
                }
                else
                {
                    return $"Error: {error}";
                }
            }
        }

        private string ExecuteTraceroute(string host)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "tracert";
                process.StartInfo.Arguments = host;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (string.IsNullOrEmpty(error))
                {
                    return output;
                }
                else
                {
                    return $"Error: {error}";
                }
            }
        }
        #endregion
    }
}
