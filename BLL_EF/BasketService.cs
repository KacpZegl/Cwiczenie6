using BLL.ServiceInterfaces;
using DAL;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_EF
{

    public class BasketService : IBasketService
        {
            private readonly WebshpContext _context;

            public BasketService(WebshpContext context)
            {
                _context = context;
            }

            public void AddProductToBasket(int productId, int userId)
            {
                var product = _context.Products.Find(productId);
                if (product == null)
                    throw new ArgumentException("Product not found.");

            if (!product.IsActive)
            {
                throw new ArgumentException("Product must be active");
            }

                var user = _context.Users.Find(userId);
                if (user == null)
                    throw new ArgumentException("User not found.");


            // Sprawdzamy, czy użytkownik ma już koszyk


            var currentProductExist = _context.BasketPositions.Where(x => x.ProductID == productId && x.UserID == userId);

            if (currentProductExist.Any())
            {
                var updateBasket = currentProductExist.First();
                updateBasket.Amount++;
            }
            else
            {
                var basket = new BasketPosition
                {
                    UserID = userId,
                    ProductID = productId,
                    Amount = 1
                };
                _context.BasketPositions.Add(basket);
            }
                _context.SaveChanges();
            }

        public void ChangeBasketPositionQuantity(int basketPositionId, int newQuantity)
        {
            var basketPosition = _context.BasketPositions.Find(basketPositionId);
            if (basketPosition == null)
                throw new ArgumentException("Basket position not found.");

            if (newQuantity <= 0)
            {
                // Jeśli nowa ilość jest mniejsza lub równa zero, usuwamy pozycję z koszyka
                _context.BasketPositions.Remove(basketPosition);
            }
            else
            {
                basketPosition.Amount = newQuantity;
            }

            _context.SaveChanges();
        }
        public void RemoveProductFromBasket(int basketPositionId)
        {
            var basketPosition = _context.BasketPositions.Find(basketPositionId);
            if (basketPosition == null)
                throw new ArgumentException("Basket position not found.");

            _context.BasketPositions.Remove(basketPosition);
            _context.SaveChanges();
        }
    }
    }
