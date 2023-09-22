using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TPA11.Models;

namespace TPA11.Models
{
    [Table("clientorder")] // Spécifier le nom de la table ici puisqu'il diffère du modèle(EF pluralise par default)
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

        // Propriété de navigation pour OrderItems (one-to-many)
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}