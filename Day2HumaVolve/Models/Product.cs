using System.ComponentModel.DataAnnotations;

namespace Day2HumaVolve.Models
{
    public class Product
    {
        public int Id { get; set; } // PK
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;

        #region Navigation Properties
        public Category Category { get; set; } = default!;
        public int CategoryId { get; set; } // FK
        #endregion
    }
}
