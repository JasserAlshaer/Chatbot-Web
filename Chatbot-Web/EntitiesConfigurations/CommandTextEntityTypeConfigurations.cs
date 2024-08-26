using Chatbot_Web.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot_Web.EntitiesConfigurations
{
    public class CommandTextEntityTypeConfigurations : IEntityTypeConfiguration<CommandText>
    {
        public void Configure(EntityTypeBuilder<CommandText> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}
