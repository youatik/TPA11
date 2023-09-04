using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPA11.Models
{
    public class LibraryItem
    {
        [Key]
        public long EanIsbn13 { get; set; }

        [Required]
        [StringLength(145)]
        public string Title { get; set; }

        [Required]
        [StringLength(123)]
        public string Creators { get; set; }

        [Required]
        [StringLength(13)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(14)]
        public string LastName { get; set; }

        [Required]
        [StringLength(4769)]
        public string Description { get; set; }

        [StringLength(37)]
        public string Publisher { get; set; }

        public DateTime? PublishDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(7, 3)")]
        public decimal Price { get; set; }

        public int Length { get; set; }
    }
}
