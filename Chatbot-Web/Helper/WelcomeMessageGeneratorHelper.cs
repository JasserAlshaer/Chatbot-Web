namespace Chatbot_Web.Helper
{
    public static class WelcomeMessageGeneratorHelper
    {
        public static string GetWelcomeMessage()
        {
            string greeting = GetGreeting();
            return $"{greeting}! Welcome to our service.";
        }

        private static string GetGreeting()
        {
            var hour = DateTime.Now.Hour;

            if (hour < 12)
            {
                return "Good morning";
            }
            else if (hour < 18)
            {
                return "Good afternoon";
            }
            else
            {
                return "Good evening";
            }
        }

    }
}
