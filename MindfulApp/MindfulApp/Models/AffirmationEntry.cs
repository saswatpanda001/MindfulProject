using System;
using System.Collections.Generic;
using System.Text;

namespace MindfulApp.Models
{
    public class AffirmationEntry
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}
