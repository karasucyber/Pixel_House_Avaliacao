using System;
using System.ComponentModel.DataAnnotations;

namespace PixelHouse.Models
{
    public class Compra
    {
        public int Id { get; set; }
        
        [Required]
        public int ProdutoId { get; set; }
        
        public Produto? Produto { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantidade { get; set; }
        
        public DateTime DataEntrada { get; set; } = DateTime.Now; // Data de entrada (compra)
    }
}
