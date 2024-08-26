using Chatbot_Web.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot_Web.EntitiesConfigurations
{
    public class ConservationEntityTypeConfigurations : IEntityTypeConfiguration<Conservation>
    {
        public void Configure(EntityTypeBuilder<Conservation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            //relationships
            builder.HasMany<Message>().WithOne().HasForeignKey(x => x.ConservationId);
        }
    }
}
