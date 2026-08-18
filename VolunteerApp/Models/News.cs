using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace VolunteerApp.Models {
    [Table("News")]
    public class News : BaseModel {
        [PrimaryKey("id")]
        public int id { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }

        [Column("title")]
        public string title { get; set; } = string.Empty;

        [Column("content")]
        public string content { get; set; } = string.Empty;

        [Column("image")]
        public string image { get; set; } = string.Empty;
    }
}
