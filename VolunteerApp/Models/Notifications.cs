using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace VolunteerApp.Models {
    [Table("Notifications")]
    public class Notifications : BaseModel {
        [PrimaryKey("id")]
        public int event_ID { get; set; }

        [Column("title")]
        public string title { get; set; }

        [Column("message")]
        public string message { get; set; }

        [Column("created_at")]
        public DateTimeOffset created_at { get; set; }
    }
}
