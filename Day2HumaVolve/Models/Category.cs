namespace Day2HumaVolve.Models
{
    public class Category
    {
        public int Id { get; set; } // PK
        public string Name { get; set; } = default!;


        #region Navigation Properties
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        #endregion
    }
}
