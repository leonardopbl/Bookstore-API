using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


namespace Bookstore_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] Book book)
        {
            var exists = System.IO.File.Exists("book.json");
            if (exists)
            {
                string existsJson = System.IO.File.ReadAllText("book.json");
                List<Book> books;
                if (string.IsNullOrEmpty(existsJson))
                {
                    books = new List<Book>();
                }
                else
                {
                    try
                    {
                        books = JsonSerializer.Deserialize<List<Book>>(existsJson) ?? new List<Book>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Erro ao desserializar JSON: {ex.Message}");
                        books = new List<Book>();
                    }
                }

                books.Add(book);

                string updatedJson = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });

                System.IO.File.WriteAllText("book.json", updatedJson);

                return Created(string.Empty, $"Livro '{book.Title}' criado com sucesso com o código '{book.Id}'!");

            }
            string json = JsonSerializer.Serialize(book, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText("book.json", json);
            return Created(string.Empty, $"Livro '{book.Title}' criado com sucesso com o código {book.Id}!");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get()
        {
            string json = System.IO.File.ReadAllText("book.json");
            try
            {
                var books = JsonSerializer.Deserialize<List<Book>>(json);
                return Ok(books);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao desserializar JSON: {ex.Message}");
                return ValidationProblem("Erro ao obter livros");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public IActionResult Put([FromQuery] string id, [FromBody] Book book)
        {
            string json = System.IO.File.ReadAllText("book.json");
            try
            {
                var books = JsonSerializer.Deserialize<List<Book>>(json);
                var bookToUpdate = books?.FirstOrDefault(b => b.Id == id);
                if (bookToUpdate == null)
                {
                    return NotFound("Livro não encontrado");
                }
                bookToUpdate.Id = bookToUpdate.Id;
                bookToUpdate.Title = book.Title ?? bookToUpdate.Title;
                bookToUpdate.Author = book.Author ?? bookToUpdate.Author;
                bookToUpdate.Genre = book.Genre ?? bookToUpdate.Genre;
                bookToUpdate.Price = book.Price != 0 ? book.Price : bookToUpdate.Price;
                bookToUpdate.Quantity = book.Quantity != 0 ? book.Quantity : bookToUpdate.Quantity;

                string updatedJson = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText("book.json", updatedJson);
                return NoContent();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao desserializar JSON: {ex.Message}");
                return ValidationProblem("Erro ao obter livros");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult Delete([FromQuery] string id)
        {
            string json = System.IO.File.ReadAllText("book.json");
            try
            {
                var books = JsonSerializer.Deserialize<List<Book>>(json);
                var bookToDelete = books?.FirstOrDefault(b => b.Id == id);
                if (bookToDelete == null)
                {
                    return NotFound("Livro não encontrado");
                }
                books?.Remove(bookToDelete);
                string updatedJson = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText("book.json", updatedJson);
                return NoContent();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao desserializar JSON: {ex.Message}");
                return ValidationProblem("Erro ao obter livros");
            }

        }
    }
}