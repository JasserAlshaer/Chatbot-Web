﻿namespace Chatbot_Web.DTOs.Authantication
{
    public class SignupDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
    }
}
