using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace VolunteerApp.Models {
    [Table("Event_Role")]
    public class EventRole : BaseModel {
        public String name;
        [Column("role_id")]
        public int role_ID { get; set; }

        [Column("event_id")]
        public int event_ID { get; set; }

        [Column("number_limit")]
        public int number_limit { get; set; }

    }
}
