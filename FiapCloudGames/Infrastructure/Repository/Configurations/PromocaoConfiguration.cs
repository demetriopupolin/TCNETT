using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Configurations
{
    public class PromocaoConfiguration : IEntityTypeConfiguration<Promocao>
    {
        public void Configure(EntityTypeBuilder<Promocao> builder)
        {
            builder.ToTable("PROMOCAO");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").ValueGeneratedNever().UseIdentityColumn();
            builder.Property(p => p.Nome).HasColumnName("NOME").HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(p => p.DataCriacao).HasColumnName("DATACRIACAO").HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.Desconto).HasColumnName("DESCONTO").HasColumnType("INT").IsRequired();
            builder.Property(p => p.DataValidade).HasColumnName("DATAVALIDADE").HasColumnType("DATETIME").IsRequired();

            builder.HasIndex(p => p.Nome)
                            .IsUnique()
                            .HasDatabaseName("UQ_PROMOCAO_NOME");

            //teste
            builder.HasMany(p => p.Pedidos)
                .WithOne(pedido => pedido.Promocao)
                .HasForeignKey(pedido => pedido.PromocaoId)
                .IsRequired(false);


        }
    }
}
