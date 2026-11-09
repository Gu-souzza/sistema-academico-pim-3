using ZeroFrame.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ZeroFrame.Infra.Data.Context
{
    // Contexto do banco de dados da aplicação.
    // Responsável por representar as tabelas e aplicar as configurações das entidades.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Carrinho> carrinhos { get; set; }
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Endereco> enderecos { get; set; }
        public DbSet<ItemCarrinho> item_Carrinhos { get; set; }
        public DbSet<ItemPedido> itemPedidos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Pedidos> pedidos { get; set; }
        public DbSet<Produto> produtos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<VariacaoProdutos> variacaoprodutos { get; set; }
        public DbSet<AvaliacaoProduto> AvaliacoesProdutos { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }
        public DbSet<BuscaRecente> BuscasRecentes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Favorito>(entity =>
            {
                entity.HasKey(item => new { item.UsuarioId, item.ProdutoId });
                entity.HasOne<Usuario>().WithMany().HasForeignKey(item => item.UsuarioId);
                entity.HasOne<Produto>().WithMany().HasForeignKey(item => item.ProdutoId);
            });
            modelBuilder.Entity<BuscaRecente>(entity =>
            {
                entity.HasKey(item => new { item.UsuarioId, item.Chave });
                entity.Property(item => item.Chave).HasMaxLength(120);
                entity.Property(item => item.Termo).HasMaxLength(120);
                entity.HasOne<Usuario>().WithMany().HasForeignKey(item => item.UsuarioId);
            });
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);    
        }
    }
}
