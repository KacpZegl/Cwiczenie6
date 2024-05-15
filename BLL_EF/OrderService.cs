using BLL.DTOModels;
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
    public class OrderService : IOrderService
    {
        private readonly WebshpContext _context;

        public OrderService(WebshpContext context)
        {
            _context = context;
        }

        public void GenerateOrderFromBasket(int userId)
{
    // Ensure userId is a valid value (since it cannot be null for value types like int).
    if (userId <= 0) // Assuming userId should be greater than 0.
    {
        throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
    }

    // Include the Product when retrieving BasketPositions
    var baskets = _context.BasketPositions
                          .Include(bp => bp.Product) // This ensures Product is not null
                          .Where(bp => bp.UserID == userId)
                          .ToList();

    if (!baskets.Any())
    {
        throw new InvalidOperationException("Basket is empty.");
    }

    // Create a new Order
    var order = new Order
    {
        UserID = userId,
        Date = DateTime.Now
        // OrderPositions is likely initialized in the Order constructor
    };

    // Add OrderPositions to the new Order
    foreach (var basketPosition in baskets)
    {
        if (basketPosition.Product == null)
        {
            // Handle the case where Product is somehow still null
            throw new InvalidOperationException("Product information is missing for a basket item.");
        }

        var orderPosition = new OrderPosition
        {
            // Don't set OrderID; EF will do this upon saving the order.
            Amount = basketPosition.Amount,
            Price = basketPosition.Product.Price * basketPosition.Amount,
            ProductID = basketPosition.ProductID // Assign ProductID directly
            // Don't need to set the Order; EF handles this when the order is added to the context.
        };

        // Add orderPosition to the Order's OrderPositions collection
        order.OrderPositions.Add(orderPosition);
    }

    // Add the new Order to the context
    _context.Orders.Add(order);
    // Remove BasketPositions now that they've been converted to OrderPositions
    _context.BasketPositions.RemoveRange(baskets);
    // Save changes to the database
    _context.SaveChanges();
}


        public IEnumerable<OrderPositionResponseDTO> GetOrderPositions(int orderId)
        {
            // Query the OrderPositions for the given orderId
            var orderPositionsQuery = _context.OrderPositions
                                              .Where(op => op.OrderID == orderId)
                                              .Include(op => op.Product); // Assuming you have a navigation property for Product

            // Project the OrderPositions to the DTO
            var orderPositions = orderPositionsQuery
                .Select(op => new OrderPositionResponseDTO
                {
                    ProductName = op.Product.Name, // Assuming Product has a Name property
                    Price = op.Price,
                    Quantity = op.Amount,
                    TotalValue = op.Price * op.Amount
                }).ToList();

            return orderPositions;
        }


        public IEnumerable<OrderResponseDTO> GetOrders(string sortBy, int? orderIdFilter, bool? paidStatusFilter)
        {
            IQueryable<Order> query = _context.Orders;

            // Apply filters if provided
            if (orderIdFilter.HasValue)
            {
                query = query.Where(o => o.ID == orderIdFilter.Value);
            }

            if (paidStatusFilter.HasValue)
            {
                query = query.Where(o => o.IsPayed == paidStatusFilter.Value);
            }

            // Apply sorting based on the sortBy parameter
            switch (sortBy?.ToLower())
            {
                case "date":
                    query = query.OrderBy(o => o.Date);
                    break;
                case "date_desc":
                    query = query.OrderByDescending(o => o.Date);
                    break;
                // Additional sorting options can be added here
                default:
                    // Default sorting (if unspecified, we'll use OrderId as default)
                    query = query.OrderBy(o => o.ID);
                    break;
            }

            // Project the result to DTOs
            var orders = query.Select(o => new OrderResponseDTO
            {
                OrderId = o.ID,
                Value = o.OrderPositions.Sum(op => op.Price * op.Amount),
                IsPaid = o.IsPayed,
                OrderDate = o.Date
            }).ToList();

            return orders;
        }


        public void PayOrder(int orderId, decimal amountPaid)
        {
            var order = _context.Orders.FirstOrDefault(o => o.ID == orderId);
            if (order == null)
                throw new ArgumentException("Order not found.");

            if (order.IsPayed)
                throw new InvalidOperationException("Order has already been paid.");

            var amountToPay = _context.OrderPositions.Where(x => x.OrderID == orderId).Sum(x => x.Amount);
            if (amountToPay == amountPaid)
            {
                // Aktualizujemy status zamówienia na opłacone
                order.IsPayed = true;

                // Zapisujemy kwotę wpłacaną przez klienta

                _context.SaveChanges();
            }
            else 
            {
                throw new Exception("Amount is not correct");
            }

        }
    }
}
