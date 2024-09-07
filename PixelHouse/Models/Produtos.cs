using System;
using System.ComponentModel.DataAnnotations;

namespace PixelHouse.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        public required string Nome { get; set; }

        public string? Descricao { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Preco { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantidadeEmEstoque { get; set; }

        [Range(0, int.MaxValue)]
        public int EstoqueMinimo { get; set; }

        [Required]
        public string? Unidade { get; set; }

        [Range(0, 100)]
        public decimal ICMS { get; set; }

        [Required]
        public string? CFOP { get; set; }

        public DateTime DataEntrada { get; set; } = DateTime.Now; // Data de entrada (compra)

        public DateTime DataSaida { get; set; } = DateTime.Now; // Data de saída (venda)

        public DateTime DataAlteracao { get; set; } = DateTime.Now; // Data de última alteração

        public string? FornecedorPreferencial { get; set; }

        public string? ProdutosAtivo { get; set; } // ativo para venda S ou N
    }
}
