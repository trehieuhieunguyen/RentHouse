using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentHouse.Models;
using RentHouse.Models.Groups;

namespace RentHouse.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<House> houses { get; set; }
        public DbSet<RoomHouse> roomHouses { get; set; }
       
        public DbSet<UserRent> UserRent { get; set; }
        public DbSet<Agreement> agreements { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<ImageUpload> imageUploads { get; set; }
        public DbSet<MessageRoom> messageRooms { get; set; }
        public DbSet<ImageUploadOfRoom> SubImageOfRoom { get; set; }
        public DbSet<HouseOfUser> houseOfUser { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<RatingStar> RatingStars {  get; set; }
        public DbSet<HistoryPay> HistoryPay { get; set; }
    }
}