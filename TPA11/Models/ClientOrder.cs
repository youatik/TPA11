using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TPA11.Models;

namespace TPA11.Models
{
    public class ClientOrder
    {
        [Key]
        public int OrderId { get; set; }

        public int? ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public DateTime? OrderDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalAmount { get; set; }

        // Navigation property for OrderItems (one-to-many)
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}