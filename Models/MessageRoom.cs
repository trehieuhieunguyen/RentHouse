using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentHouse.Models
{
    public class MessageRoom
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("RoomHouseId")]
        public int RoomHouseId { get; set; }
        public RoomHouse RoomHouse { get; set; }

        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Status { get; set; }
        public string Text { get; set; }

       
    }
}
