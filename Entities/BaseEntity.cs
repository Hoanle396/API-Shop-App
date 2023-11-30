using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Entities {
    public class BaseEntity {
        [Key, Column("id"),DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}
