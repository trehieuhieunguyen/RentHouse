using System.ComponentModel.DataAnnotations;

namespace RentHouse.Models
{
    public class TestError
    {
        [Key]
        public int Id { get; set; }
        public string text { get; set; }
    }
}
