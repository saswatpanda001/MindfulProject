using System;

namespace MindfulApp.Models
{
    // Inherits all properties of MeditationSession and adds IsSelected for bulk selection
    public class MeditationSessionSelectable : MeditationSession
    {
        public bool IsSelected { get; set; } = false;
       
        public string UserName { get; set; }

    }
}
