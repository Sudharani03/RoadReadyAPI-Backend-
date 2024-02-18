using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoadReady.Models
{
    [Table("Validations")]
    public class Validation
    {
        [Key]
        [Required]
        public string Username { get; set; }
        [Required]
        public byte[] Password { get; set; }
        public string? Role { get; set; }
        public byte[]? Key { get; set; }
        public User? User { get; set; }
        public Admin? Admin { get; set; }
    }
}
