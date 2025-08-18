namespace FirstWebApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        //public Product (int Id, string Name, decimal Price, int Stock)
        //{
        //    this.Id = Id;
        //    this.Name = Name;
        //    this.Price = Price;
        //    this.Stock = Stock;
        //}
    }
}
