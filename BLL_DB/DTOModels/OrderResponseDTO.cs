namespace BLL_DB.DTOModels
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }
    }
}
