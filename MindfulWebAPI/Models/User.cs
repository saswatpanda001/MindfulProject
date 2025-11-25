using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MindfulWebAPI.Models;

namespace MindfulWebAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [MinLength(4, ErrorMessage = "Full name must be at least 4 characters.")]
        public string FullName { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        [MinLength(4, ErrorMessage = "Email must be at least 4 characters.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string Phone { get; set; }

        [MaxLength(100)]
        [MinLength(4, ErrorMessage = "Location must be at least 4 characters.")]
        public string Location { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters.")]
        public string Password { get; set; }

        [Required]
        [RegularExpression("^(User|Admin)$", ErrorMessage = "Role must be either User or Admin.")]
        public string Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
