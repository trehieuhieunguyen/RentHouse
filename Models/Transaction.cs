using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentHouse.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserRentId")]
        public int UserRentId { get; set; }
        public UserRent userRent { get; set; }

        public DateTime dateTimeCreate { get; set; }

        public double Deposit { get; set; }
        public double Discount { get; set; }
    }
}
