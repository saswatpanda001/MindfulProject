using System;
using System.Collections.Generic;
using System.Text;

namespace MindfulApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
