namespace RentHouse.Models.ViewModel
{
    public class RoomCommentVM
    {
        public ICollection<MessageRoom> commentRooms { get; set; }
        public RoomHouse roomHouse {  get; set; }
        public ICollection<ImageUploadOfRoom> imageUploadOfRooms { get; set; }
    }
}
