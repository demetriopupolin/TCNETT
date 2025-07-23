using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("USUARIO");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(p => p.DataCriacao).HasColumnName("DATACRIACAO").HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.Nome).HasColumnName("NOME").HasColumnType("VARCHAR(100)").IsRequired().HasConversion(
                    p => p.ToUpper(), 
                    p => p);
            builder.Property(p => p.Email).HasColumnName("EMAIL").HasColumnType("VARCHAR(100)").IsRequired().HasConversion(
                    p => p.ToLower(), 
                    p => p );
            builder.Property(p => p.Senha).HasColumnName("SENHA").HasColumnType("VARCHAR(8)").IsRequired();
            builder.Property(p => p.Nivel).HasColumnName("NIVEL").HasColumnType("CHAR(1)").IsRequired().HasConversion(
                  p => char.ToUpper(p),
                  p => p);


            //  teste
            builder.HasMany(u => u.Pedidos)
                   .WithOne(p => p.Usuario)
                   .HasForeignKey(p => p.UsuarioId);



        }
    }
}
