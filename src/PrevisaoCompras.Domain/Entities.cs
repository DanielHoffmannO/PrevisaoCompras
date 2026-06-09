namespace PrevisaoCompras.Domain.Entities;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Unidade { get; set; } = "un";
}

public class HistoricoCompra
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public DateTime DataCompra { get; set; }
    public Produto Produto { get; set; } = null!;
}

public class Previsao
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public int QuantidadePrevista { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public DateTime GeradaEm { get; set; }
    public Produto Produto { get; set; } = null!;
}
