using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MindfulWebAPI.Models
{
    public class MoodEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [MaxLength(20)]
        [MinLength(3, ErrorMessage = "Mood must be at least 3 characters.")]
        public string Mood { get; set; }   // Great, Good, Okay, Low, Sad


        [Required]
        [MaxLength(500)]
        [MinLength(4, ErrorMessage = "Note must be at least 3 characters.")]
        public string Note { get; set; }


        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Foreign key
        [Required]
        public int UserId { get; set; }

       
    }
}
