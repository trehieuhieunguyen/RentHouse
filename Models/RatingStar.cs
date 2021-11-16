using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentHouse.Models
{
    public class RatingStar
    {
        [Key]
        public int Id {  get; set; }
        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("RoomHouseId")]
        public int RoomHouseId { get; set; }
        public RoomHouse RoomHouse { get; set; }
        public int Star { get; set; }
        public DateTime CreateTime {  get; set; }
    }
}
