using APITracker_Service.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APITracker_Service.Mappings;
public class EnderecoApiMap : IEntityTypeConfiguration<EnderecoAPI>
{
    public void Configure(EntityTypeBuilder<EnderecoAPI> builder)
    {
        builder
            .HasKey(c => c.Id);

        builder.ToTable("EnderecoApi");

        builder.Property(p => p.Descricao).HasMaxLength(200);

        builder.Property(p => p.Endereco).HasMaxLength(200);

        builder.Property(p => p.Error)
            .HasDefaultValue("")
            .HasMaxLength(4000);

        builder.Property(p => p.Body).HasMaxLength(4000);
    }
}