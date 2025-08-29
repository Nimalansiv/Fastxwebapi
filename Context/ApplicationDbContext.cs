using Microsoft.EntityFrameworkCore;
using FastxWebApi.Models;
using System.Security.Cryptography;
using System.Text;
using Route = FastxWebApi.Models.Route;

namespace FastxWebApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<BookingSeat> BookingSeats { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Route> Routes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>().HasOne(u => u.Role)
                .WithMany(r => r.User)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<User>().HasMany(u => u.Buses)
                .WithOne(b => b.BusOperator)
                .HasForeignKey(b => b.BusOperatorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Bus>(entity =>
            {
                entity.HasKey(b => b.BusId);
                entity.HasOne(b => b.BusOperator)
                    .WithMany(u => u.Buses)
                    .HasForeignKey(b => b.BusOperatorId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(b => b.Routes)
                    .WithOne(r => r.Bus)
                    .HasForeignKey(r => r.BusId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.ToTable("Buses");
            });

            
            modelBuilder.Entity<Bus>()
                .HasMany(b => b.Seats)
                .WithOne(s => s.Bus)
                .HasForeignKey(s => s.BusId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Route>().HasKey(r => r.RouteId);
            modelBuilder.Entity<Seat>().HasKey(s => s.SeatId);
            modelBuilder.Entity<Booking>().HasKey(b => b.BookingId);
            modelBuilder.Entity<Booking>().HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Payment>().HasKey(p => p.PaymentId);
            modelBuilder.Entity<Payment>().HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Refund>().HasKey(r => r.RefundId);
            modelBuilder.Entity<Refund>().HasOne(r => r.Booking)
                .WithOne(b => b.Refund)
                .HasForeignKey<Refund>(r => r.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Refund>().HasOne(r => r.ProcessedByUser)
                .WithMany(u => u.ProcessedRefunds)
                .HasForeignKey(r => r.ProcessedBy)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BookingSeat>().HasKey(bs => bs.Id);
            modelBuilder.Entity<BookingSeat>().ToTable("BookingsSeats");
            modelBuilder.Entity<BookingSeat>().HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BookingSeat>().HasOne(bs => bs.Seat)
                .WithMany(s => s.BookingSeats)
                .HasForeignKey(bs => bs.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

           
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "User" },
                new Role { RoleId = 2, RoleName = "Admin" },
                new Role { RoleId = 3, RoleName = "Bus Operator" }
            );
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Name = "Nimalan", Email = "Nimal@fastx.com", Password = CreatePasswordHash("Heisen@33"), HashKey = CreatePasswordHashKey(), Gender = "Male", ContactNumber = "9876543210", Address = "CDM", RoleId = 1, BusId = null, CreatedAt = DateTime.Now },
                new User { UserId = 2, Name = "Guru", Email = "GM@fastx.com", Password = CreatePasswordHash("Guru@12"), HashKey = CreatePasswordHashKey(), Gender = "Male", ContactNumber = "785423654", Address = "LA, california", RoleId = 2, BusId = null, CreatedAt = DateTime.Now },
                new User { UserId = 3, Name = "Driver Thor", Email = "thordriver@fastx.com", Password = CreatePasswordHash("driverkey"), HashKey = CreatePasswordHashKey(), Gender = "Male", ContactNumber = "4552155741", Address = "Heaven block 1", RoleId = 3, BusId = null, CreatedAt = DateTime.Now },
                new User { UserId = 4, Name = "Driver Kumar", Email = "Kumardriver@fast.com", Password = CreatePasswordHash("iamdriverkumar"), HashKey = CreatePasswordHashKey(), Gender = "Male", ContactNumber = "7845213698", Address = "Chennai", RoleId = 3, BusId = null, CreatedAt = DateTime.Now },
                new User { UserId = 5, Name = "Sivan", Email = "sivan@fast.com", Password = CreatePasswordHash("iamsivangod"), HashKey = CreatePasswordHashKey(), Gender = "Male", ContactNumber = "7458961235", Address = "Chennai", RoleId = 2, BusId = null, CreatedAt = DateTime.Now },
                 new User { UserId = 6, Name = "sankar", Email = "sankar@fastx.com", Password = CreatePasswordHash("iamsankar"), HashKey = CreatePasswordHashKey(), Gender = "Male", ContactNumber = "7854123695", Address = "CDM", RoleId = 1, BusId = null, CreatedAt = DateTime.Now },
                new User { UserId = 7, Name = "Venkat", Email = "venkat@gmail.com", Password = CreatePasswordHash("iamvenkat"), HashKey = CreatePasswordHashKey(), Gender = "Male", ContactNumber = "7436985212", Address = "Chepauk", RoleId = 1, BusId = null, CreatedAt = DateTime.Now }


            );
            modelBuilder.Entity<Bus>().HasData(
                new Bus { BusId = 1, BusName = "Volvo", BusNumber = "KA-01-AB-8754", BusType = "AC Sleeper", TotalSeats = 40, Amenities = "Wi-Fi, Water Bottle,TV", BusOperatorId = 3 },
                new Bus { BusId = 2, BusName = "BENZ", BusNumber = "TN-02-CD-9999", BusType = "Non-AC", TotalSeats = 45, Amenities = "Wi-Fi", BusOperatorId = 3 },
                new Bus { BusId = 3, BusName = "Bharat", BusNumber = "TM-08-CM-2022", BusType = "Non-AC", TotalSeats = 25, Amenities = "Wi-Fi", BusOperatorId = 3 },
                new Bus { BusId = 4, BusName = "ECR express", BusNumber = "SN-02-TR-2354", BusType = "Non-AC", TotalSeats = 18, Amenities = "Wi-Fi", BusOperatorId = 4 },
                new Bus { BusId = 5, BusName = "Rameshwaram exp", BusNumber = "ER-02-NM-5421", BusType = "AC", TotalSeats = 20, Amenities = "Wi-Fi", BusOperatorId = 4 },
                new Bus { BusId = 6, BusName = "Kovai exp", BusNumber = "YR-03-RF-8976", BusType = "AC", TotalSeats = 13, Amenities = "Wi-Fi", BusOperatorId = 4 }

            );
            modelBuilder.Entity<Route>().HasData(
                new Route { RouteId = 1, BusId = 1, Origin = "Bangalore", Destination = "Chidambaram", DepartureTime = new DateTime(2025, 9, 27, 22, 0, 0), ArrivalTime = new DateTime(2025, 9, 30, 6, 0, 0), Fare = 1500.00m },
                new Route { RouteId = 2, BusId = 2, Origin = "Chennai", Destination = "Cuddalore", DepartureTime = new DateTime(2025, 9, 4, 23, 30, 0), ArrivalTime = new DateTime(2025, 9, 6, 7, 30, 0), Fare = 800.00m },
                new Route { RouteId = 3, BusId = 3, Origin = "Chennai", Destination = "CDM", DepartureTime = new DateTime(2025, 9, 21, 23, 30, 0), ArrivalTime = new DateTime(2025, 9, 23, 7, 30, 0), Fare = 600.00m },
                new Route { RouteId = 4, BusId = 4, Origin = "Chennai", Destination = "Cuddalore", DepartureTime = new DateTime(2025, 9, 9, 23, 30, 0), ArrivalTime = new DateTime(2025, 9, 10, 7, 30, 0), Fare = 800.00m },
                new Route { RouteId = 5, BusId = 5, Origin = "Rameshwaram", Destination = "CDM", DepartureTime = new DateTime(2025, 9, 11, 23, 30, 0), ArrivalTime = new DateTime(2025, 9, 13, 7, 30, 0), Fare = 1800.00m },
                new Route { RouteId = 6, BusId = 6, Origin = "Kovai", Destination = "CDM", DepartureTime = new DateTime(2025, 9, 15, 23, 30, 0), ArrivalTime = new DateTime(2025, 9, 18, 7, 30, 0), Fare = 2800.00m }


            );

            modelBuilder.Entity<Seat>().HasData(
     
     new Seat { SeatId = 1, BusId = 1, IsBooked = false, RouteId = 1, SeatNumber = "A1", Status = "Active" },
     new Seat { SeatId = 2, BusId = 1, IsBooked = false, RouteId = 1, SeatNumber = "A2", Status = "Active" },
     new Seat { SeatId = 3, BusId = 1, IsBooked = false, RouteId = 1, SeatNumber = "A3", Status = "Active" },
     new Seat { SeatId = 4, BusId = 1, IsBooked = false, RouteId = 1, SeatNumber = "A4", Status = "Active" },
     new Seat { SeatId = 5, BusId = 1, IsBooked = false, RouteId = 1, SeatNumber = "A5", Status = "Active" },
     new Seat { SeatId = 6, BusId = 1, IsBooked = false, RouteId = 1, SeatNumber = "A6", Status = "Active" },
     new Seat { SeatId = 7, BusId = 1, IsBooked = false, RouteId = 1, SeatNumber = "A7", Status = "Active" },
     new Seat { SeatId = 8, BusId = 1, IsBooked = false, RouteId = 1, SeatNumber = "A8", Status = "Active" },
     new Seat { SeatId = 9, BusId = 1, IsBooked = false, RouteId = 1, SeatNumber = "A9", Status = "Active" },
     new Seat { SeatId = 10, BusId = 1, IsBooked = false, RouteId = 1, SeatNumber = "A10", Status = "Active" },

   
     new Seat { SeatId = 11, BusId = 2, IsBooked = false, RouteId = 2, SeatNumber = "B1", Status = "Active" },
     new Seat { SeatId = 12, BusId = 2, IsBooked = false, RouteId = 2, SeatNumber = "B2", Status = "Active" },
     new Seat { SeatId = 13, BusId = 2, IsBooked = false, RouteId = 2, SeatNumber = "B3", Status = "Active" },
     new Seat { SeatId = 14, BusId = 2, IsBooked = false, RouteId = 2, SeatNumber = "B4", Status = "Active" },
     new Seat { SeatId = 15, BusId = 2, IsBooked = false, RouteId = 2, SeatNumber = "B5", Status = "Active" },
     new Seat { SeatId = 16, BusId = 2, IsBooked = false, RouteId = 2, SeatNumber = "B6", Status = "Active" },
     new Seat { SeatId = 17, BusId = 2, IsBooked = false, RouteId = 2, SeatNumber = "B7", Status = "Active" },
     new Seat { SeatId = 18, BusId = 2, IsBooked = false, RouteId = 2, SeatNumber = "B8", Status = "Active" },
     new Seat { SeatId = 19, BusId = 2, IsBooked = false, RouteId = 2, SeatNumber = "B9", Status = "Active" },
     new Seat { SeatId = 20, BusId = 2, IsBooked = false, RouteId = 2, SeatNumber = "B10", Status = "Active" },

     
     new Seat { SeatId = 21, BusId = 3, IsBooked = false, RouteId = 3, SeatNumber = "C1", Status = "Active" },
     new Seat { SeatId = 22, BusId = 3, IsBooked = false, RouteId = 3, SeatNumber = "C2", Status = "Active" },
     new Seat { SeatId = 23, BusId = 3, IsBooked = false, RouteId = 3, SeatNumber = "C3", Status = "Active" },
     new Seat { SeatId = 24, BusId = 3, IsBooked = false, RouteId = 3, SeatNumber = "C4", Status = "Active" },
     new Seat { SeatId = 25, BusId = 3, IsBooked = false, RouteId = 3, SeatNumber = "C5", Status = "Active" },
     new Seat { SeatId = 26, BusId = 3, IsBooked = false, RouteId = 3, SeatNumber = "C6", Status = "Active" },
     new Seat { SeatId = 27, BusId = 3, IsBooked = false, RouteId = 3, SeatNumber = "C7", Status = "Active" },
     new Seat { SeatId = 28, BusId = 3, IsBooked = false, RouteId = 3, SeatNumber = "C8", Status = "Active" },
     new Seat { SeatId = 29, BusId = 3, IsBooked = false, RouteId = 3, SeatNumber = "C9", Status = "Active" },
     new Seat { SeatId = 30, BusId = 3, IsBooked = false, RouteId = 3, SeatNumber = "C10", Status = "Active" },

     
     new Seat { SeatId = 31, BusId = 4, IsBooked = false, RouteId = 4, SeatNumber = "D1", Status = "Active" },
     new Seat { SeatId = 32, BusId = 4, IsBooked = false, RouteId = 4, SeatNumber = "D2", Status = "Active" },
     new Seat { SeatId = 33, BusId = 4, IsBooked = false, RouteId = 4, SeatNumber = "D3", Status = "Active" },
     new Seat { SeatId = 34, BusId = 4, IsBooked = false, RouteId = 4, SeatNumber = "D4", Status = "Active" },
     new Seat { SeatId = 35, BusId = 4, IsBooked = false, RouteId = 4, SeatNumber = "D5", Status = "Active" },
     new Seat { SeatId = 36, BusId = 4, IsBooked = false, RouteId = 4, SeatNumber = "D6", Status = "Active" },
     new Seat { SeatId = 37, BusId = 4, IsBooked = false, RouteId = 4, SeatNumber = "D7", Status = "Active" },
     new Seat { SeatId = 38, BusId = 4, IsBooked = false, RouteId = 4, SeatNumber = "D8", Status = "Active" },
     new Seat { SeatId = 39, BusId = 4, IsBooked = false, RouteId = 4, SeatNumber = "D9", Status = "Active" },
     new Seat { SeatId = 40, BusId = 4, IsBooked = false, RouteId = 4, SeatNumber = "D10", Status = "Active" },

    
     new Seat { SeatId = 41, BusId = 5, IsBooked = false, RouteId = 5, SeatNumber = "E1", Status = "Active" },
     new Seat { SeatId = 42, BusId = 5, IsBooked = false, RouteId = 5, SeatNumber = "E2", Status = "Active" },
     new Seat { SeatId = 43, BusId = 5, IsBooked = false, RouteId = 5, SeatNumber = "E3", Status = "Active" },
     new Seat { SeatId = 44, BusId = 5, IsBooked = false, RouteId = 5, SeatNumber = "E4", Status = "Active" },
     new Seat { SeatId = 45, BusId = 5, IsBooked = false, RouteId = 5, SeatNumber = "E5", Status = "Active" },
     new Seat { SeatId = 46, BusId = 5, IsBooked = false, RouteId = 5, SeatNumber = "E6", Status = "Active" },
     new Seat { SeatId = 47, BusId = 5, IsBooked = false, RouteId = 5, SeatNumber = "E7", Status = "Active" },
     new Seat { SeatId = 48, BusId = 5, IsBooked = false, RouteId = 5, SeatNumber = "E8", Status = "Active" },
     new Seat { SeatId = 49, BusId = 5, IsBooked = false, RouteId = 5, SeatNumber = "E9", Status = "Active" },
     new Seat { SeatId = 50, BusId = 5, IsBooked = false, RouteId = 5, SeatNumber = "E10", Status = "Active" },

     
     new Seat { SeatId = 51, BusId = 6, IsBooked = false, RouteId = 6, SeatNumber = "F1", Status = "Active" },
     new Seat { SeatId = 52, BusId = 6, IsBooked = false, RouteId = 6, SeatNumber = "F2", Status = "Active" },
     new Seat { SeatId = 53, BusId = 6, IsBooked = false, RouteId = 6, SeatNumber = "F3", Status = "Active" },
     new Seat { SeatId = 54, BusId = 6, IsBooked = false, RouteId = 6, SeatNumber = "F4", Status = "Active" },
     new Seat { SeatId = 55, BusId = 6, IsBooked = false, RouteId = 6, SeatNumber = "F5", Status = "Active" },
     new Seat { SeatId = 56, BusId = 6, IsBooked = false, RouteId = 6, SeatNumber = "F6", Status = "Active" },
     new Seat { SeatId = 57, BusId = 6, IsBooked = false, RouteId = 6, SeatNumber = "F7", Status = "Active" },
     new Seat { SeatId = 58, BusId = 6, IsBooked = false, RouteId = 6, SeatNumber = "F8", Status = "Active" },
     new Seat { SeatId = 59, BusId = 6, IsBooked = false, RouteId = 6, SeatNumber = "F9", Status = "Active" },
     new Seat { SeatId = 60, BusId = 6, IsBooked = false, RouteId = 6, SeatNumber = "F10", Status = "Active" }
 );
           

            modelBuilder.Entity<Booking>().HasData(
    new Booking { BookingId = 1, UserId = 1, RouteId = 1, BookingDate = new DateTime(2025, 8, 25, 10, 0, 0), NoOfSeats = 1, TotalFare = 1500.00m, Status = "Cancelled", IsDeleted = true },
    new Booking { BookingId = 2, UserId = 1, RouteId = 2, BookingDate = new DateTime(2025, 8, 26, 11, 30, 0), NoOfSeats = 2, TotalFare = 1600.00m, Status = "Booked", IsDeleted = false },
    new Booking { BookingId = 3, UserId = 1, RouteId = 2, BookingDate = new DateTime(2025, 8, 24, 12, 0, 0), NoOfSeats = 2, TotalFare = 1600.00m, Status = "Booked", IsDeleted = false },
    new Booking { BookingId = 4, UserId = 6, RouteId = 3, BookingDate = new DateTime(2025, 8, 23, 13, 0, 0), NoOfSeats = 3, TotalFare = 1800.00m, Status = "Cancelled", IsDeleted = true },
    new Booking { BookingId = 5, UserId = 6, RouteId = 4, BookingDate = new DateTime(2025, 8, 22, 14, 0, 0), NoOfSeats = 1, TotalFare = 800.00m, Status = "Booked", IsDeleted = false },
    new Booking { BookingId = 6, UserId = 7, RouteId = 5, BookingDate = new DateTime(2025, 8, 22, 15, 0, 0), NoOfSeats = 2, TotalFare = 3600.00m, Status = "Cancelled", IsDeleted = true },
    new Booking { BookingId = 7, UserId = 7, RouteId = 6, BookingDate = new DateTime(2025, 8, 26, 16, 0, 0), NoOfSeats = 1, TotalFare = 2800.00m, Status = "Booked", IsDeleted = false }
);
            modelBuilder.Entity<Refund>().HasData(
                new Refund { RefundId = 1, BookingId = 1, RefundAmount = 1500.00m, RefundDate = DateTime.Now, UserId = 1, Status = "Pending", ProcessedBy = null },
                new Refund { RefundId = 2, BookingId = 4, RefundAmount = 1800.00m, RefundDate = DateTime.Now, UserId = 6, Status = "Pending", ProcessedBy = 2 },
                new Refund { RefundId = 3, BookingId = 6, RefundAmount = 3600.00m, RefundDate = DateTime.Now, UserId = 7, Status = "Pending", ProcessedBy = null }
            );
            modelBuilder.Entity<Payment>().HasData(
                new Payment { PaymentId = 1, BookingId = 1, Amount = 1500.00m, PaymentMethod = "Credit Card", PaymentDate = new DateTime(2025, 8, 9, 10, 0, 0), Status = "Completed", UserId = 1 },
                new Payment { PaymentId = 2, BookingId = 2, Amount = 1600.00m, PaymentMethod = "UPI", PaymentDate = new DateTime(2025, 8, 9, 11, 30, 0), Status = "Completed", UserId = 1 },
                 new Payment { PaymentId = 3, BookingId = 3, Amount = 1600.00m, PaymentMethod = "Credit Card", PaymentDate = new DateTime(2025, 9, 26, 12, 0, 0), Status = "Completed", UserId = 1 },
    new Payment { PaymentId = 4, BookingId = 4, Amount = 1800.00m, PaymentMethod = "UPI", PaymentDate = new DateTime(2025, 8, 23, 13, 0, 0), Status = "Completed", UserId = 6 },
    new Payment { PaymentId = 5, BookingId = 5, Amount = 800.00m, PaymentMethod = "Credit Card", PaymentDate = new DateTime(2025, 8, 26, 14, 0, 0), Status = "Completed", UserId = 6 },
    new Payment { PaymentId = 6, BookingId = 6, Amount = 3600.00m, PaymentMethod = "Debit Card", PaymentDate = new DateTime(2025, 9, 26, 15, 0, 0), Status = "Completed", UserId = 7 },
    new Payment { PaymentId = 7, BookingId = 7, Amount = 2800.00m, PaymentMethod = "UPI", PaymentDate = new DateTime(2025, 9, 26, 16, 0, 0), Status = "Completed", UserId = 7 }
    






            );

            base.OnModelCreating(modelBuilder);
        }

        private byte[] CreatePasswordHashKey()
        {
            using var hmac = new HMACSHA256();
            return hmac.Key;
        }

        private byte[] CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA256();
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}