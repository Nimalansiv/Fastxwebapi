using Microsoft.EntityFrameworkCore;
using FastxWebApi.Models;


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

        public DbSet<BookingSeat> BookingsSeats {  get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Models.Route> Routes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);


            modelBuilder.Entity<User>().HasOne(u => u.Role)
                .WithMany(r => r.User) 

                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<User>().HasOne(u => u.Bus)
               .WithOne(b => b.BusOperator)

               .HasForeignKey<User>(u => u.BusId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bus>().HasKey(b => b.BusId);

            modelBuilder.Entity<Bus>().HasMany(b => b.Routes)
                .WithOne(r => r.Bus)
                 .HasForeignKey(r => r.BusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Route>().HasKey(r => r.RouteId);

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

            modelBuilder.Entity<Bus>().HasData(
               new Bus { BusId = 1, BusName = "Volvo ", BusNumber = "KA-01-AB-8754", BusType = "AC Sleeper", TotalSeats = 40, Amenities = "Wi-Fi, Water Bottle,TV" },

               new Bus { BusId = 2, BusName = "BENZ  ", BusNumber = "TN-02-CD-9999", BusType = "Non-AC", TotalSeats = 45, Amenities = "Wi-Fi" }
           );

            modelBuilder.Entity<Models.Route>().HasData(
               new Models.Route
               {
                   RouteId = 1,BusId = 1,Origin = "Bangalore",Destination = "Chidambaram",DepartureTime = new DateTime(2025, 8, 2, 22, 0, 0),ArrivalTime = new DateTime(2025, 8, 3, 6, 0, 0),Fare = 1500.00m
               },
               new Models.Route
               {
                   RouteId = 2, BusId = 2,Origin = "Chennai",Destination = "Cuddalore",DepartureTime = new DateTime(2025, 8, 2, 23, 30, 0),ArrivalTime = new DateTime(2025, 8, 3, 7, 30, 0),Fare = 800.00m
               }
           );

            modelBuilder.Entity<Seat>().HasData(
                 new Seat { SeatId = 1, RouteId = 1, SeatNumber = "A1" },new Seat { SeatId = 2, RouteId = 1, SeatNumber = "A2" },new Seat { SeatId = 3, RouteId = 1, SeatNumber = "B1" },

               new Seat { SeatId = 4, RouteId = 2, SeatNumber = "C1" },new Seat { SeatId = 5, RouteId = 2, SeatNumber = "C2" }
             );

            modelBuilder.Entity<User>().HasData(
               new User
               {
                    UserId = 1,Name = "Nimalan",Email = "Nimal@fastx.com",Password = "SayMyName",Gender = "Male",
                      ContactNumber = "9876543210",Address = "CDM",RoleId = 1, 
                       BusId = null,CreatedAt = DateTime.Now
               },
                new User
                {
                  UserId = 2,Name = "Guru",Email = "GM@fastx.com",Password = "GuruHere",Gender = "Male",ContactNumber = "785423654",Address = "LA,california",RoleId = 2, 
                    BusId = null,CreatedAt = DateTime.Now
                 },
                 new User
                 {
                     UserId = 3,Name = "Driver Thor",Email = "thordriver@fastx.com",Password = "Braceforimpact",Gender = "Male",ContactNumber = "4552155741",Address = "Heaven block 1",RoleId = 3, 
        
                      BusId = 1,CreatedAt = DateTime.Now
                 }
            );






        }


    }
}
