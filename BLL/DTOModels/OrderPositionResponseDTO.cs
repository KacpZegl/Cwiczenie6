namespace BLL.DTOModels
{
    public class OrderPositionResponseDTO
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalValue { get; set; }
    }
}
