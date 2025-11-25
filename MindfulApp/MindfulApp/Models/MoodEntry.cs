using System;
using System.Collections.Generic;
using System.Text;

namespace MindfulApp.Models
{
    public class MoodEntry
    {
        public int Id { get; set; }
        public string Mood { get; set; }   // Great, Good, Okay, Low, Sad
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
