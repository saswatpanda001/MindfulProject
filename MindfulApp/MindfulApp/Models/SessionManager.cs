using MindfulApp.Models;

namespace MindfulApp.Services
{
    public static class SessionManager
    {
        // Stores the currently logged-in user
        public static User LoggedInUser { get; set; }
    }
}
