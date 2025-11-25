using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MindfulWebAPI.Models
{
    public class AffirmationEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(4, ErrorMessage = "Category must be at least 4 characters.")]
        public string Category { get; set; }

        [Required]
        [MaxLength(500)]
        [MinLength(4, ErrorMessage = "Text must be at least 4 characters.")]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        [Required]
        public int UserId { get; set; }


    }

}
