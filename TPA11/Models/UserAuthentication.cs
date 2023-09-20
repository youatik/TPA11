using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TPA11.Models;

//comment

namespace TPA11.Models
{
    public class UserAuthentication
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
