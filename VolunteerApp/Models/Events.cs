using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace VolunteerApp.Models {
    [Table("Events")]
    public class Events : BaseModel {
            [PrimaryKey("event_id")]
            public int event_ID { get; set; }

            [Column("name")]
            public string name { get; set; }

            [Column("description")]
            public string description { get; set; }

            [Column("link")]
            public string link { get; set; }

            [Column("date")]
            public DateOnly date { get; set; }

            [Column("time_begin")]
            public TimeOnly time_begin { get; set; }

            [Column("time_end")]
            public TimeOnly time_end { get; set; }
    }
}
