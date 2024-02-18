using System.ComponentModel.DataAnnotations;

namespace RoadReady.Models
{
    public class RentalStore : IEquatable<RentalStore>
    {
        public int StoreId { get; set; }
        [Required(ErrorMessage = "PickUpStoreLocation is Required")]
        public string PickUpLocation { get; set; }
        [Required(ErrorMessage = "DropOffStoreLocation is Required")]
        public string DropOffLocation { get; set; }

        // Navigation property for the one-to-many relationship
        public ICollection<CarStore>? CarStore { get; set; }

        //Default Constructor 
        public RentalStore()
        {
            StoreId= 0;
        }

        //Parameterized Constructor
        public RentalStore(int storeId, string pickUpLocation, string dropOffLocation)
        {
            StoreId = storeId;
            PickUpLocation = pickUpLocation;
            DropOffLocation = dropOffLocation;
        }
        public RentalStore( string pickUpLocation, string dropOffLocation)
        {
            
            PickUpLocation = pickUpLocation;
            DropOffLocation = dropOffLocation;
        }
        public bool Equals(RentalStore? other)
        {
            var rentalStore = other ?? new RentalStore();
            return this.StoreId.Equals(rentalStore.StoreId);
        }
    }
}
