namespace RentHouse.Models.ViewModel
{
    public class RoomVM
    {
        public RoomHouse roomHouse {  get; set; }
        public ICollection<ImageUploadOfRoom> imageUploadOfRooms {  get; set; }
        public ImageUploadOfRoom imageUploadOfRoom { get; set; }
        public IList<IFormFile> formFiles { get; set; }
    }
}
