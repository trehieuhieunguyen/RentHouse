namespace RentHouse.Models.ViewModel
{
    public class RoomInHouseVM
    {
        public ICollection<RoomHouse> roomHouses {  get; set; }
        public ICollection<RoomHouse> roomHousesModal { get; set; }
        public int AllRoom {  get; set; }
    }
}
