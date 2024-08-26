using Chatbot_Web.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot_Web.EntitiesConfigurations
{
    public class UserTypeEntityTypeConfigurations : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            //relationships
            builder.HasMany<User>().WithOne().HasForeignKey(x => x.UserTypeId);
        }
    }
}
