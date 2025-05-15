namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    public class CreateCustomer
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Segment { get; set; } // Example: "Monday", "Tuesday", etc.
    }
}
