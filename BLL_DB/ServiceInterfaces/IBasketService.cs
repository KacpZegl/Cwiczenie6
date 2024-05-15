using System;

namespace BLL_DB.ServiceInterfaces
{
    public interface IBasketService
    {
        void AddProductToBasket(int productId, int userId);
        void ChangeBasketPositionQuantity(int productId, int userId, int newQuantity); // Poprawiona sygnatura
        void RemoveProductFromBasket(int productId, int userId); // Poprawiona sygnatura
    }
}
