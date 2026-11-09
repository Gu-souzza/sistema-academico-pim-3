using ZeroFrame.Domain.Entidades;
using ZeroFrame.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ZeroFrame.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZeroFrame.Infra.Data.Repositorios
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly ApplicationDbContext _context;

        public EnderecoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Busca todos os endereços cadastrados no banco
        public async Task<List<Endereco>> ObterTodosAsync()
        {
            return await _context.enderecos
                .Include(e => e.Usuario)
                .ToListAsync();
        }

        // Busca um endereço específico pelo Id
        public async Task<Endereco?> ObterPorIdAsync(int id)
        {
            return await _context.enderecos
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Busca todos os endereços de um usuário específico

        public async Task<Endereco?> ObterPorUsuarioIdAsync(int usuarioId)
        {
            return await _context.enderecos
                .Include(e => e.Usuario)
                .Where(e => e.UsuarioId == usuarioId && e.Ativo)
                .OrderByDescending(e => e.Principal).ThenBy(e => e.Id)
                .FirstOrDefaultAsync();
        }

        // Conta os enderecos cadastrados para aplicar o limite de criacao no Service.
        public async Task<int> CountByUsuarioIdAsync(int usuarioId)
        {
            return await _context.enderecos
                .CountAsync(e => e.UsuarioId == usuarioId && e.Ativo);
        }

        // Adiciona um novo endereço no banco de dados
        public async Task AdicionarAsync(Endereco endereco)
        {
            await _context.enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();
        }

        // Atualiza um endereço existente no banco de dados
        public async Task AtualizarAsync(Endereco endereco)
        {
            _context.enderecos.Update(endereco);
            await _context.SaveChangesAsync();
        }

        // Remove um endereço existente no banco de dados
        public async Task RemoverAsync(int id)
        {
            var endereco = await _context.enderecos.FindAsync(id);

            if (endereco is null)
                return;

            endereco.Ativo = false;
            endereco.Principal = false;
            await _context.SaveChangesAsync();
        }
    }
}
