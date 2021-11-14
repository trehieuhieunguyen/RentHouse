using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentHouse.Models
{
    public class UserRent
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [ForeignKey("RoomId")]
        public int RoomId { get; set; }
        public RoomHouse RoomHouse { get; set; }

        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        

        [Phone(ErrorMessage = "Please Enter Phone")]
        public string Phone { get; set; }
    }
}
