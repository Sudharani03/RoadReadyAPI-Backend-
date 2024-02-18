using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoadReady.Models
{
    public class CarStore
    {

        //ForeignKey property for the one-to-many relationship
        public Car? Car { get; set; }
        public int CarId { get; set; }

        //ForeignKey property for the one-to-many relationship
        public RentalStore? RentalStore { get; set; }
        public int StoreId { get; set; }
        
        //Default Constructor

        public CarStore()
        {

        }

        //Parameterized Constructor 

        public CarStore(int carId, int storeId)
        {
            CarId = carId;
            StoreId = storeId;
        }
    }
}
