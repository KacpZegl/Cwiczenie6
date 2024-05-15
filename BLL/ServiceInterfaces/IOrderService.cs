using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IOrderService
    {
        void GenerateOrderFromBasket(int userId);
        void PayOrder(int orderId, decimal amountPaid);
        IEnumerable<OrderResponseDTO> GetOrders(
            string sortBy,
            int? orderIdFilter,
            bool? paidStatusFilter);
        IEnumerable<OrderPositionResponseDTO> GetOrderPositions(int orderId);
    }
}
