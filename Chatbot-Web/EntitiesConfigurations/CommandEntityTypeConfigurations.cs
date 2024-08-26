using Chatbot_Web.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot_Web.EntitiesConfigurations
{
    public class CommandEntityTypeConfigurations : IEntityTypeConfiguration<Command>
    {
        public void Configure(EntityTypeBuilder<Command> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            //relationships
            builder.HasMany<CommandText>().WithOne().HasForeignKey(x => x.CommandId);
        }
    }
}
