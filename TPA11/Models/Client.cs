﻿using System;
using System.ComponentModel.DataAnnotations;


namespace TPA11.Models
{
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

        // Navigation property for ClientOrders (one-to-many)
        public ICollection<ClientOrder> ClientOrders { get; set; }

        // Navigation property for Payments (one-to-many)
        public ICollection<Payment> Payments { get; set; }

        // Navigation property for ShoppingCartItems (one-to-many)
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        // Navigation property for UserAuthentication (one-to-one)
        public UserAuthentication UserAuthentication { get; set; }

    }
}
