using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeroFrame.Domain.Entidades;
using ZeroFrame.Domain.Interfaces;
using ZeroFrame.Infra.Data.Context;

namespace ZeroFrame.API.Controllers;

[ApiController]
[Authorize]
[Route("api/usuarios/me")]
public class DadosCompartilhadosController(ApplicationDbContext banco, IUnitOfWork transacoes) : ControllerBase
{
    private int UsuarioId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpDelete]
    public async Task<IActionResult> DesativarConta()
    {
        await banco.Usuarios.Where(usuario => usuario.Id == UsuarioId)
            .ExecuteUpdateAsync(set => set.SetProperty(usuario => usuario.Ativo, false));
        return NoContent();
    }

    [HttpGet("enderecos")]
    public async Task<IActionResult> ListarEnderecos()
    {
        var enderecos = await banco.enderecos
            .Where(item => item.UsuarioId == UsuarioId && item.Ativo)
            .OrderByDescending(item => item.Principal).ThenBy(item => item.Id)
            .Select(item => new { item.Id, item.Rua, item.Numero, item.Bairro, item.Cidade, item.Estado, Cep = item.CEP, item.Complemento })
            .ToListAsync();
        return Ok(enderecos.Select((item, indice) => new
        {
            item.Id, item.Rua, item.Numero, item.Bairro, item.Cidade, item.Estado, item.Cep, item.Complemento,
            Principal = indice == 0
        }));
    }

    [HttpPut("enderecos/{id:int}/principal")]
    public Task<IActionResult> TornarEnderecoPrincipal(int id)
    {
        var usuarioId = UsuarioId;
        return transacoes.ExecuteInTransactionAsync<IActionResult>(async () =>
        {
            var enderecos = await banco.enderecos
                .Where(item => item.UsuarioId == usuarioId && item.Ativo).OrderBy(item => item.Id).ToListAsync();
            if (!enderecos.Any(item => item.Id == id))
                return NotFound("Endereço não encontrado.");
            foreach (var item in enderecos)
                item.Principal = item.Id == id;
            await banco.SaveChangesAsync();
            return NoContent();
        });
    }

    [HttpGet("favoritos")]
    public async Task<int[]> ListarFavoritos() => await banco.Favoritos
        .Where(item => item.UsuarioId == UsuarioId)
        .OrderBy(item => item.CriadoEm).ThenBy(item => item.ProdutoId)
        .Select(item => item.ProdutoId).ToArrayAsync();

    [HttpPut("favoritos/{produtoId:int}")]
    public async Task<IActionResult> AdicionarFavorito(int produtoId)
    {
        if (!await banco.produtos.AnyAsync(produto => produto.Id == produtoId && produto.Ativo))
            return NotFound("Produto não encontrado.");

        // HOLDLOCK torna a inserção repetível mesmo se dois dispositivos enviarem o mesmo favorito.
        var usuarioId = UsuarioId;
        var agora = DateTime.UtcNow;
        await banco.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO Favoritos (UsuarioId, ProdutoId, CriadoEm)
            SELECT {usuarioId}, {produtoId}, {agora}
            WHERE NOT EXISTS (
                SELECT 1 FROM Favoritos WITH (UPDLOCK, HOLDLOCK)
                WHERE UsuarioId = {usuarioId} AND ProdutoId = {produtoId}
            )
            """);
        return NoContent();
    }

    [HttpDelete("favoritos/{produtoId:int}")]
    public async Task<IActionResult> RemoverFavorito(int produtoId)
    {
        await banco.Favoritos.Where(item => item.UsuarioId == UsuarioId && item.ProdutoId == produtoId)
            .ExecuteDeleteAsync();
        return NoContent();
    }

    [HttpGet("buscas-recentes")]
    public async Task<string[]> ListarBuscas() => await banco.BuscasRecentes
        .Where(item => item.UsuarioId == UsuarioId).OrderByDescending(item => item.AtualizadaEm)
        .ThenBy(item => item.Chave).Take(5).Select(item => item.Termo).ToArrayAsync();

    [HttpPost("buscas-recentes")]
    public async Task<IActionResult> RegistrarBusca([FromBody] TermoBusca dados)
    {
        var termo = dados.Termo.Trim();
        if (termo.Length == 0) return BadRequest("Informe o termo da busca.");
        var usuarioId = UsuarioId;
        var chave = termo.ToLowerInvariant();
        var agora = DateTime.UtcNow;
        await transacoes.ExecuteInTransactionAsync(async () =>
        {
            await banco.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE BuscasRecentes WITH (UPDLOCK, HOLDLOCK)
                SET Termo = {termo}, AtualizadaEm = {agora}
                WHERE UsuarioId = {usuarioId} AND Chave = {chave};
                IF @@ROWCOUNT = 0
                    INSERT INTO BuscasRecentes (UsuarioId, Chave, Termo, AtualizadaEm)
                    VALUES ({usuarioId}, {chave}, {termo}, {agora});
                """);
            await banco.BuscasRecentes.Where(item => item.UsuarioId == usuarioId)
                .OrderByDescending(item => item.AtualizadaEm).ThenBy(item => item.Chave).Skip(5)
                .ExecuteDeleteAsync();
        });
        return Ok(await ListarBuscas());
    }

    [HttpDelete("buscas-recentes")]
    public async Task<string[]> RemoverBusca([FromQuery] string termo)
    {
        var chave = termo.Trim().ToLowerInvariant();
        await banco.BuscasRecentes.Where(item => item.UsuarioId == UsuarioId && item.Chave == chave)
            .ExecuteDeleteAsync();
        return await ListarBuscas();
    }

    public record TermoBusca([Required, StringLength(120)] string Termo);
}
