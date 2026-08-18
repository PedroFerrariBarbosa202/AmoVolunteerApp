using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace VolunteerApp.Models {
    [Table("Roles")]
    public class Roles : BaseModel {
        [PrimaryKey("role_id")]
        public int role_ID { get; set; }

        [Column("name")]
        public string name { get; set; }
    }
}
