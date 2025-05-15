using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(100, ErrorMessage = "Address cannot be longer than 100 characters.")]
        public string Address { get; set; } = string.Empty;
        [Required]

        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Segment { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }= string.Empty;

        public int DeliveryDateId { get; set; } = 0;

        public List<CustomerDeliveryDates> CustomerDeliveryDates { get; set; } = new List<CustomerDeliveryDates>();
     
        public List<Order> Orders { get; set; } = new List<Order>();



    }
}
