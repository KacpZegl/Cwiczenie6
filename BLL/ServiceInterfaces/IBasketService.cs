using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IBasketService
    {
        void AddProductToBasket(int productId, int userId);
        void ChangeBasketPositionQuantity(int basketPositionId, int newQuantity);
        void RemoveProductFromBasket(int basketPositionId);
    }
}
