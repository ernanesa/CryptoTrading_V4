using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trading.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(75)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(75)")]
        public string TypeID { get; set; }
        [Column(TypeName = "varchar(75)")]
        public string Secret { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public string AccessToken { get; set; }
    }
}