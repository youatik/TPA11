using System;
using System.ComponentModel.DataAnnotations;


namespace TPA11.Models
{
    [Table("client")] // Spécifier le nom de la table ici puisqu'il diffère du modèle(EF pluralise par default)
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        // Propriété de navigation pour ClientOrders (one-to-many)
        public ICollection<ClientOrder> ClientOrders { get; set; }

        // Propriété de navigation pour Payments (one-to-many)
        public ICollection<Payment> Payments { get; set; }

        // Propriété de navigation pour ShoppingCartItems (one-to-many)
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        // Propriété de navigation pour UserAuthentication (one-to-one)
        public UserAuthentication UserAuthentication { get; set; }

    }
}
