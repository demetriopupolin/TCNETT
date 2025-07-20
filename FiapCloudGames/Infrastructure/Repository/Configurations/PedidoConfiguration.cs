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
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("PEDIDO");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(p => p.DataCriacao).HasColumnName("DATACRIACAO").HasColumnType("DATETIME").IsRequired();

            builder.Property(p => p.UsuarioId).HasColumnName("USUARIOID").HasColumnType("INT").IsRequired();
            builder.Property(p => p.JogoId).HasColumnName("JOGOID").HasColumnType("INT").IsRequired();
            builder.Property(p => p.PromocaoId).HasColumnName("PROMOCAOID").HasColumnType("INT");

            // Relação: Pedido -> Usuario
            builder.HasOne(p => p.Usuario)
                   .WithMany(u => u.Pedidos)
                   .HasForeignKey(p => p.UsuarioId);

            // Relação: Pedido -> Jogo
            builder.HasOne(p => p.Jogo)
                   .WithMany(j => j.Pedidos)
                   .HasForeignKey(p => p.JogoId);

            // Relação: Pedido -> Promocao (opcional)
            builder.HasOne(p => p.Promocao)
                   .WithMany(promo => promo.Pedidos)
                   .HasForeignKey(p => p.PromocaoId)
                   .IsRequired(false);


        }
    }

}
