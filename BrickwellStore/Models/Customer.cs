using System.ComponentModel.DataAnnotations;

namespace BrickwellStore.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }  
        public string Country { get; set; }
        public string Phone { get; set; } 
        public string Email { get; set; }
        public string Zip { get; set; }
        public int CCNumber { get; set; }
        public string CCDate { get; set; }
        public int CCCode { get; set; }
        public string CardholderName { get; set; }
    }
}
