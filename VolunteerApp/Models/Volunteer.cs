using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace VolunteerApp.Models {
        [Table("Volunteer")]
        public class Volunteer : BaseModel {
            [PrimaryKey("volunteer_id")]
            public int volunteer_ID { get; set; }

            [Column("name")]
            public string name { get; set; }

            [Column("email")]
            public string email { get; set; }

            [Column("password")]
            public string password { get; set; }

            [Column("birth_date")]
            public DateOnly age { get; set; }

            [Column("profession")]
            public string profession { get; set; }

            [Column("company")]
            public string company { get; set; }

            [Column("phone")]
            public string phone { get; set; }

            [Column("user_img")]
            public string user_img { get; set; }

            [Column("is_validated")]
            public bool is_validated { get; set; } = false;

            [Column("solicitation_seen")]
            public bool solicitation_seen { get; set; } = false;

            [Column("logged_in")]
            public bool logged_in { get; set; }
    }
}
