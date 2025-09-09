using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroSystem.Domain.Entities;

namespace MicroSystem.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Email).IsRequired();
        
        builder.Property(x => x.PasswordHash).IsRequired();
        
        builder.Property(x => x.UserName).IsRequired();
    }
}