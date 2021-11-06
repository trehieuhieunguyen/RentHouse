using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentHouse.Models
{
    public class Agreement
    {
        [Key] 
        public int AgreementID { get; set; }

        [ForeignKey("UserRentId")]
        public int UserRentId { get; set; }
        public UserRent userRent { get; set; }

        public string Description { get; set; }

        [Required]
        public double MonneyForMonth { get; set; }

    }
}
