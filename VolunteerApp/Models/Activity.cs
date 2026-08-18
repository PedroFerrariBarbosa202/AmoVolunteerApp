using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace VolunteerApp.Models {
    [Table("Activity")]
    public class Activity : BaseModel {
        [PrimaryKey("id")]
        public int ID { get; set; }

        [Column("volunteer_id")]
        public int volunteer_ID { get; set; }

        [Column("created_at")]
        public DateTime? created_at { get; set; }
        [Column("finished_at")]
        public DateTime? finished_at { get; set; }
    }
}
