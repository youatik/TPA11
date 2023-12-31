﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TPA11.Models
{
    [Table("library")] // Specify the table name here
    public class LibraryItem
    {
        [Key]
        public long ean_isbn13 { get; set; }

        [StringLength(145)]
        public string Title { get; set; }

        [StringLength(123)]
        public string Creators { get; set; }

        [StringLength(13)]
        public string FirstName { get; set; }

        [StringLength(14)]
        public string LastName { get; set; }

        [StringLength(4769)]
        public string Description { get; set; }

        [StringLength(37)]
        public string Publisher { get; set; }

        public DateTime? PublishDate { get; set; }

        [Column(TypeName = "decimal(7, 3)")]
        public decimal Price { get; set; }

        public int Length { get; set; }

        // Navigation property for OrderItems (one-to-many)
        public ICollection<OrderItem> OrderItems { get; set; }

        // Navigation property for ShoppingCartItems (one-to-many)
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
