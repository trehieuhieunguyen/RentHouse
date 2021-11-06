namespace RentHouse.Models.ViewModel
{
    public class HouseVM
    {
        public ImageUpload imageUpload {  get; set; }
        public ICollection<ImageUpload> imageUploads {  get; set; }
        public IList<IFormFile> formFiles { get; set; }
        public House house {  get; set; }
    }
}
