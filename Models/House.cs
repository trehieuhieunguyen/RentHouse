using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentHouse.Models
{
    public class House
    {
        [Key]
        public int Id { get; set; }
        public string NameHourse { get; set; }
        public string ImgUrl { get; set; }
        [Required(ErrorMessage = "Please Enter ward")]
        public string Ward { get; set; }
        [Required(ErrorMessage = "Please Enter city")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please Enter district")]
        public string District { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime TimeUpdate { get; set; }
        public int AllRoom { get; set; }
        public bool isDeleted { get; set; }
       
    }
}
