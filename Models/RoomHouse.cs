using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentHouse.Models
{
    public class RoomHouse
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("HouseId")]
        public int HouseId { get; set; }
        public House house { get; set; }

        [Required(ErrorMessage = "Please Enter RoomNumber")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Please Enter RoomSize")]
        public double RoomSize { get; set; }

        [Required(ErrorMessage = "Please Enter Windowns")]
        public int Windowns { get; set; }
        public double PriceRent { get; set; }
        public bool StatusRent { get; set; }
        public string UrlImg { get; set; }
        public DateTime UpdateTime { get; set; }
        public double Star { get;set;  }
    }
}
