using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TPA11.Models;

namespace TPA11.Models
{
    public class ShoppingCartItem
    {
        [Key]
        public int Id { get; set; }

        public int? ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public long? EanIsbn13 { get; set; }

        [ForeignKey("EanIsbn13")]
        public LibraryItem LibraryItem { get; set; }

        public int? Quantity { get; set; }
    }
}
