namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    public class OrderMonthSummaryDto
    {
        public int TotalOrders { get; set; }
        public decimal RevenueThisWeek { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public decimal RevenueThisYear { get; set; }
    }
}
