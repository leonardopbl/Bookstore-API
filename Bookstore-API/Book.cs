using System.ComponentModel.DataAnnotations;

namespace Bookstore_API
{
    public class Book
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; }

        public Book()
        {
            if (string.IsNullOrEmpty(Id))
            {

                Id = Guid.NewGuid().ToString();
            }
        }
    }
}