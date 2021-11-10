using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentHouse.Models
{
    public class ImageUploadOfRoom
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("RoomHouseId")]
        public int RoomHouseId { get; set; }
        public RoomHouse RoomHouse { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }
    }
}
