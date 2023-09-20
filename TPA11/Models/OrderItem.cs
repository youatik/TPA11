using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TPA11.Models;

namespace TPA11.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public ClientOrder Order { get; set; }

        public long? ean_isbn13 { get; set; }

        [ForeignKey("ean_isbn13")]
        public LibraryItem LibraryItem { get; set; }

        public int? Quantity { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Price { get; set; }

        public int? ClientId { get; set; }

       
    }
}
