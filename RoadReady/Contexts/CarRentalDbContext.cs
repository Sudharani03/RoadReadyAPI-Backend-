using Microsoft.EntityFrameworkCore;
using RoadReady.Models;


namespace RoadReady.Contexts
{
    public class CarRentalDbContext:DbContext
    {
        public CarRentalDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<RentalStore> RentalStores { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CarStore> CarStore { get; set; }
        public DbSet<Validation> Validations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configuration goes here
            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(c => c.CarId);

                // Configure the one-to-many relationship using Fluent API
                modelBuilder.Entity<Car>()
                        .HasOne(d => d.Discount)
                        .WithMany(c => c.Cars)
                        .HasForeignKey(d => d.DiscountId)
                        .IsRequired(false);// Allow DiscountId to be nullable in Car table
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasKey(d => d.DiscountId);

                
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
            });

            modelBuilder.Entity<RentalStore>(entity =>
            {
                entity.HasKey(rs => rs.StoreId);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.ReservationId);

                //one to one relation with payment 

                entity.HasOne(p => p.Payment)
                      .WithOne(r => r.Reservation)
                      .HasForeignKey<Payment>(r => r.ReservationId)
                      .OnDelete(DeleteBehavior.NoAction);

                //one to many relation with User

                entity.HasOne(u => u.User)
                     .WithMany(r => r.Reservations)
                     .HasForeignKey(u => u.UserId)
                     .OnDelete(DeleteBehavior.NoAction); 

                //one to many relation with car 

                entity.HasOne(c => c.Car)
                    .WithMany(r => r.Reservations)
                    .HasForeignKey(c => c.CarId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(r => r.AppliedDiscounts)
                    .WithOne(d => d.Reservation)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(a => a.AdminId);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);

                entity.HasOne(u => u.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.Car)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(c => c.CarId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.DiscountId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);// Allow DiscountId to be nullable in Payment table
            });


            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(rv => rv.ReviewId);

                entity.HasOne(u => u.User)
                    .WithMany(rv => rv.Reviews)
                    .HasForeignKey(u => u.UserId);

                entity.HasOne(c => c.Car)
                    .WithMany(rv => rv.Reviews)
                    .HasForeignKey(c => c.CarId);
            });

          
            


        //junction table

        modelBuilder.Entity<CarStore>(entity =>
            {
                entity.HasKey(cs => new { cs.StoreId, cs.CarId }); // defines composite primary key 

                entity.HasOne(cs => cs.Car)
                    .WithMany(c => c.CarStore)
                    .HasForeignKey(cs => cs.CarId);

                entity.HasOne(cs => cs.RentalStore)
                    .WithMany(l => l.CarStore)
                    .HasForeignKey(cs => cs.StoreId);

            });

        }
    }
}
