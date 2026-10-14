using System.ComponentModel.DataAnnotations;

namespace Day2HumaVolve.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;
    }
}
