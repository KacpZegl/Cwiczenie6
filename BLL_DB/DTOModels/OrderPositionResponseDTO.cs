namespace BLL_DB.DTOModels
{
    public class OrderPositionResponseDTO
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalValue { get; set; }
    }
}
