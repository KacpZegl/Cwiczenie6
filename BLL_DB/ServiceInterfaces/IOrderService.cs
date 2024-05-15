using BLL_DB.DTOModels;
using System.Collections.Generic;

namespace BLL_DB.ServiceInterfaces
{
    public interface IOrderService
    {
        void GenerateOrderFromBasket(int userId);
        IEnumerable<OrderPositionResponseDTO> GetOrderPositions(int orderId);
        IEnumerable<OrderResponseDTO> GetOrders(string sortBy, int? orderIdFilter, bool? paidStatusFilter);
        void PayOrder(int orderId, decimal amountPaid);
    }
}
