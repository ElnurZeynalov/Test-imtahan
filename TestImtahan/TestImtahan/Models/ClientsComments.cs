using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestImtahan.Models
{
    public class ClientsComments
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string Comment { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
    }
}
