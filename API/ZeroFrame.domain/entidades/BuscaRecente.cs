namespace ZeroFrame.Domain.Entidades;

public class BuscaRecente
{
    public int UsuarioId { get; set; }
    public string Chave { get; set; } = string.Empty;
    public string Termo { get; set; } = string.Empty;
    public DateTime AtualizadaEm { get; set; }
}
