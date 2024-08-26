using Chatbot_Web.Entities;
using Chatbot_Web.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Chatbot_Web.Context
{
    public class ChatbotContext : DbContext
    {
        public ChatbotContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserTypeEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new CommandEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new CommandTextEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new ConservationEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new MessageEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new VerificationCodeEntityTypeConfigurations());

        }

        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<CommandText> CommandTexts { get; set; }
        public DbSet<Conservation> Conservations { get; set; }  
        public DbSet<Message> Messages { get; set; }    
        public DbSet<VerificationCode> VerificationCodes { get; set; }  
    }
}
