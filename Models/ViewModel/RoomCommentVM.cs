namespace RentHouse.Models.ViewModel
{
    public class RoomCommentVM
    {
        public ICollection<MessageRoom> commentRooms { get; set; }
        public RoomHouse roomHouse {  get; set; }
    }
}
