using Microsoft.EntityFrameworkCore;
using FastxWebApi.Models;
using System.Security.Cryptography;
using System.Text;
using Route = FastxWebApi.Models.Route;



namespace FastxWebApi.Context
{
    public class ApplicationDbContext:DbContext
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

            modelBuilder.Entity<User>()
                .HasMany(u => u.Buses)
                .WithOne(b => b.BusOperator)
                .HasForeignKey(b => b.BusOperatorId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Bus>(entity =>
            {
                entity.HasKey(b => b.BusId);

               entity.HasOne(b => b.BusOperator)
                  .WithMany(u=>u.Buses)  
                  .HasForeignKey(b => b.BusOperatorId)
                  .OnDelete(DeleteBehavior.Restrict); 

                entity.HasMany(b => b.Routes)
                  .WithOne(r => r.Bus)
                  .HasForeignKey(r => r.BusId)
                  .OnDelete(DeleteBehavior.Restrict);

                  entity.ToTable("Buses");
            });

            
            modelBuilder.Entity<Route>().HasKey(r => r.RouteId);

            
            modelBuilder.Entity<Seat>().HasKey(s => s.SeatId);

            modelBuilder.Entity<Seat>().HasOne(s => s.Route)
                .WithMany(r => r.Seats)
                .HasForeignKey(s => s.RouteId)
                .OnDelete(DeleteBehavior.Restrict);

            
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
                new User
                {
                    UserId = 1,Name = "Nimalan",Email = "Nimal@fastx.com",Password = CreatePasswordHash("Heisen@33"),HashKey = CreatePasswordHashKey(),
                    Gender = "Male",ContactNumber = "9876543210",Address = "CDM",RoleId = 1,BusId = null,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    UserId = 2,Name = "Guru",Email = "GM@fastx.com",Password = CreatePasswordHash("Guru@12"),HashKey = CreatePasswordHashKey(),
                    Gender = "Male",ContactNumber = "785423654",Address = "LA, california",RoleId = 2,
                    BusId = null,CreatedAt = DateTime.Now
                },
                new User
                {
                    UserId = 3,Name = "Driver Thor",Email = "thordriver@fastx.com",Password = CreatePasswordHash("driverkey"),HashKey = CreatePasswordHashKey(),Gender = "Male",ContactNumber = "4552155741",
                    Address = "Heaven block 1",RoleId = 3,BusId = 1,CreatedAt = DateTime.Now
                }
            );

            
            modelBuilder.Entity<Bus>().HasData(
                new Bus { BusId = 1, BusName = "Volvo", BusNumber = "KA-01-AB-8754", BusType = "AC Sleeper", TotalSeats = 40, Amenities = "Wi-Fi, Water Bottle,TV" },


                new Bus { BusId = 2, BusName = "BENZ", BusNumber = "TN-02-CD-9999", BusType = "Non-AC", TotalSeats = 45, Amenities = "Wi-Fi" },

                 new Bus { BusId = 3, BusName = "Bharat", BusNumber = "TM-08-CM-2022", BusType = "Non-AC", TotalSeats = 25, Amenities = "Wi-Fi" }
            );

           
            modelBuilder.Entity<Route>().HasData(
                new Route
                {
                    RouteId = 1,BusId = 1,Origin = "Bangalore",Destination = "Chidambaram",
                    DepartureTime = new DateTime(2025, 8, 2, 22, 0, 0),ArrivalTime = new DateTime(2025, 8, 3, 6, 0, 0),Fare = 1500.00m
                },
                new Route
                {
                    RouteId = 2,BusId = 2,
                    Origin = "Chennai",Destination = "Cuddalore",DepartureTime = new DateTime(2025, 8, 2, 23, 30, 0),
                    ArrivalTime = new DateTime(2025, 8, 3, 7, 30, 0),Fare = 800.00m
                },
                  new Route
                  {
                      RouteId = 3,
                      BusId = 3,
                      Origin = "Chennai",
                      Destination = "CDM",
                      DepartureTime = new DateTime(2025, 8, 2, 23, 30, 0),
                      ArrivalTime = new DateTime(2025, 8, 3, 7, 30, 0),
                      Fare = 600.00m
                  }
            );
           


            modelBuilder.Entity<Seat>().HasData(
                new Seat { SeatId = 1, RouteId = 1, SeatNumber = "A1" },

                new Seat { SeatId = 2, RouteId = 1, SeatNumber = "A2" },
                new Seat { SeatId = 3, RouteId = 1, SeatNumber = "B1" },


                new Seat { SeatId = 4, RouteId = 2, SeatNumber = "C1" },
                new Seat { SeatId = 5, RouteId = 2, SeatNumber = "C2" },
                 new Seat { SeatId = 6, RouteId = 3, SeatNumber = "D1" },
                new Seat { SeatId = 7, RouteId = 3, SeatNumber = "D2" }

            );

            
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = 1,UserId = 1,RouteId = 1,BookingDate = DateTime.Now.AddDays(-1),NoOfSeats = 1,
                    TotalFare = 1500.00m,Status = "Cancelled",IsDeleted = true
                },
                new Booking
                {
                    BookingId = 2,UserId = 1,RouteId = 2,BookingDate = DateTime.Now.AddDays(-1),NoOfSeats = 2,TotalFare = 1600.00m,
                    Status = "Booked",IsDeleted = false
                },
                  new Booking
                  {
                      BookingId = 3,UserId = 1,RouteId = 2,BookingDate = DateTime.Now.AddDays(-1),
                      NoOfSeats = 2,TotalFare = 1600.00m,Status = "Booked",IsDeleted = false
                  }

            );

          
            modelBuilder.Entity<Refund>().HasData(
                new Refund
                {
                    RefundId = 1,BookingId = 1,RefundAmount = 1500.00m,RefundDate = DateTime.Now,UserId = 1,
                    Status = "Pending",ProcessedBy = null
                },
                new Refund
                {
                    RefundId = 2,BookingId = 2,RefundAmount = 800.00m,RefundDate = DateTime.Now,UserId = 1,Status = "Approved", ProcessedBy = 2
                },
                 new Refund
                 {
                     RefundId = 3,BookingId = 3,RefundAmount = 900.00m,RefundDate = DateTime.Now,
                     UserId = 1,Status = "Pending",ProcessedBy = null
                 }
            );
            modelBuilder.Entity<Payment>().HasData(
              new Payment
              {
                  PaymentId = 1,
                  BookingId = 1,
                  Amount = 1500.00m,
                  PaymentMethod = "Credit Card",
                  PaymentDate = new DateTime(2025, 8, 9, 10, 0, 0),
                  Status = "Completed",
                  UserId = 1
              },
               new Payment
               {
                   PaymentId = 2,
                   BookingId = 2,
                   Amount = 800.00m,
                   PaymentMethod = "UPI",
                   PaymentDate = new DateTime(2025, 8, 9, 11, 30, 0),
                   Status = "Completed",
                   UserId = 1
               }
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
