using System;
using System.Collections.Generic;
using System.Text;

namespace MindfulApp.Models
{
    public class MeditationSession
    {
        public int Id { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
