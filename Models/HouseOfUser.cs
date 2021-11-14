using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentHouse.Models
{
    public class HouseOfUser
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("HouseId")]
        public int HouseId { get; set; }
        public House house { get; set; }
        public bool Status { get; set; }
    }
}
