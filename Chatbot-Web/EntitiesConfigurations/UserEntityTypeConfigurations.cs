using Chatbot_Web.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot_Web.EntitiesConfigurations
{
    public class UserEntityTypeConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            //Optional Data 
            builder.Property(x => x.Phone).IsRequired(false);
            builder.Property(x => x.IsEmailVerified).HasDefaultValue(false);
            builder.Property(x => x.IsLoggedIn).HasDefaultValue(false);
            builder.Property(x => x.UserTypeId).HasDefaultValue(1);
            //relationships
            builder.HasMany<Conservation>().WithOne().HasForeignKey(x => x.UserId);
        }
    }
}
