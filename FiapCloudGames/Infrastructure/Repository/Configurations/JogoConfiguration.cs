using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Configurations
{
    public class JogoConfiguration : IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
            builder.ToTable("JOGO");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(p => p.DataCriacao).HasColumnName("DATACRIACAO").HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.Nome).HasColumnName("NOME").HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(p => p.AnoLancamento).HasColumnName("ANOLANCAMENTO").HasColumnType("INT").IsRequired();
            builder.Property(p => p.PrecoBase).HasColumnName("PRECOBASE").HasColumnType("DECIMAL").IsRequired();
            
            //teste
            builder.HasMany(j => j.Pedidos)
                   .WithOne(p => p.Jogo)
                   .HasForeignKey(p => p.JogoId);

        }
    }
}
