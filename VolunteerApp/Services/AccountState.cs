
namespace VolunteerApp.Services {
    static class AccountState {
        public static Models.Volunteer volunteerData { get; set; } = new Models.Volunteer();
        public static List<Models.Sector> sectors { get; set; } = new();
        public static List<Models.Events> events { get; set; } = new();

        // default declaration of volunteer
        public static readonly Models.Volunteer volunteerDefault = new Models.Volunteer {
            logged_in = false,
            volunteer_ID = 0,
            name = "Not logged In",
            email = "None",
            password = "None",
            age = DateOnly.MinValue,
            profession = "None",
            company = "None",
            phone = "None",
            user_img = "",
            is_validated = false,
            solicitation_seen = false
        };
    }
}
