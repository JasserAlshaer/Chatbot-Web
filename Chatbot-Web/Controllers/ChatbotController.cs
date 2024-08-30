using Chatbot_Web.Context;
using Chatbot_Web.DTOs.Messages;
using Chatbot_Web.Entities;
using Chatbot_Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var conser = _context.Conservations.Where(x => x.IsClosed == false &&
            x.UserId == userId).FirstOrDefault();
            if (conser != null)
            {
               
                Message msgEntity1 = new Message()
                {
                    ConservationId = conser.Id,
                    IsResponseFromChatbot = false,
                    Text = title,
                    CreationDate = DateTime.Now
                };
                _context.Add(msgEntity1);
                _context.SaveChanges();
                string message = WelcomeMessageGeneratorHelper.GetWelcomeMessage();
                Message msgEntity = new Message()
                {
                    ConservationId = conser.Id,
                    IsResponseFromChatbot = true,
                    Text = message,
                    CreationDate = DateTime.Now
                };
                _context.Add(msgEntity);
                _context.SaveChanges();
                return Ok(message);
            }
            else
            {
                return StatusCode(500, "There is an active Conservations");
            }

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
        public async Task<IActionResult> ExecuteCommand([FromBody] SendMessageDTO input)
        {
            var conservattion = _context.Conservations.Where(x => x.Id == input.ConservationId
            && x.IsClosed == false).FirstOrDefault();
            if (conservattion != null)
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
                    string command = await DetermineCommandAsync(input.Text);
                    if (command.Equals("Unkown"))
                    {
                        commandResult = "Message not recognized.";
                        CreationMessage(commandResult, input.ConservationId);
                        return BadRequest(commandResult);
                    }
                    else
                    {

                        string website = ExtractWebsite(input.Text);
                        switch (command)
                        {
                            case "ping":

                                commandResult = await ExecuteRouteCommand($"ping {website}");
                                break;
                            case "tracert":
                                commandResult = await ExecuteRouteCommand($"tracert {website}");
                                break;
                            case "netstat":
                                commandResult = await ExecuteRouteCommand("netstat -an");
                                break;
                            case "speedtest":
                                commandResult = await ExecuteRouteCommand("speedtest");
                                break;
                            case "nslookup":
                                commandResult = await ExecuteRouteCommand($"nslookup {website}");
                                break;
                            case "ipconfig":
                                commandResult = await ExecuteRouteCommand("ipconfig /all");
                                break;
                            case "getmac":
                                commandResult = await ExecuteRouteCommand("getmac");
                                break;
                            case "systeminfo":
                                commandResult = await ExecuteRouteCommand("systeminfo");
                                break;
                            case "tasklist":
                                commandResult = await ExecuteRouteCommand("tasklist");
                                break;
                            case "netsh":
                                commandResult = await ExecuteRouteCommand("netsh interface show interface");
                                break;
                            case "netsh advfirewall":
                                commandResult = await ExecuteRouteCommand("netsh advfirewall show allprofiles state");
                                break;
                            default:
                                commandResult = "Sorry, I didn't understand that command.";
                                CreationMessage(commandResult, input.ConservationId);
                                return BadRequest(commandResult);
                        }
                        CreationMessage(commandResult, input.ConservationId);
                        return Ok(commandResult);
                    }

                }
            }
            else
            {
                return StatusCode(500, "Please Create New Conservation");
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
        private async Task<string> ExecuteRouteCommand(string command)
        {
            try
            {
                if (command.Equals("speedtest"))
                {
                    string speedTestCommand = "speedtest";
                    string arguments = "";

                    // Run the speedtest command and capture the output
                    //string output = await RunCommandAsync(speedTestCommand, arguments);

                    return "now i can't regonize this command";
                }
                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/C {command}";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    return string.IsNullOrEmpty(error) ? output : $"Error: {error}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
        private async Task<string> RunCommandAsync(string fileName, string arguments)
        {
            return await Task.Run(() =>
            {
                Process process = new Process();
                process.StartInfo.WorkingDirectory = @"C:\Users\USER\AppData\Local\Programs\Python\Python312\Scripts";
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true; // This line hides the command prompt window
                process.StartInfo.RedirectStandardError = true;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }

                return output;
            });
        }
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
        private async Task<string> DetermineCommandAsync(string userInput)
        {
            // Normalize user input
            userInput = userInput.ToLower();

            var matchedPhrase = await _context.CommandTexts
                    .Where(p => userInput.Contains(p.Text.ToLower()))
                    .Select(p => p.CommandId)
                    .FirstOrDefaultAsync();

            if (matchedPhrase != 0)
            {
                var commandText = _context.Commands.Where(x => x.Id == matchedPhrase)
                    .FirstOrDefault();
                if (commandText != null)
                    return commandText.Name;
                else
                    return "Unkown";
            }
            else
            {
                return "unknown";
            }
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
