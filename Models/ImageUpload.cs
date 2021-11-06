using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentHouse.Models
{
    public class ImageUpload
    {
       
            [Key]
            public int Id { get; set; }

            [ForeignKey("HouseId")]
            public int HouseId { get; set; }
            public House house { get; set; }

            public string Name { get; set; }
            public string Image { get; set; }
       
    }
}
