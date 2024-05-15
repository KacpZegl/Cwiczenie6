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
    public class ProductService : IProductService
    {
        private readonly WebshpContext _context;

        public ProductService(WebshpContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductResponseDTO> GetProducts(
            string sortBy,
            string filterByName,
            string filterByGroupName,
            int? filterByGroupId,
            bool includeInactive)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Group);

            if (!includeInactive)
                query = query.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(filterByName))
                query = query.Where(p => p.Name.Contains(filterByName));

            if (!string.IsNullOrEmpty(filterByGroupName))
                query = query.Where(p => p.Group.Name.Contains(filterByGroupName));

            if (filterByGroupId.HasValue)
                query = query.Where(p => p.GroupID == filterByGroupId);

            switch (sortBy?.ToLower() ?? "default")
            {
                case "name":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "price":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "group":
                    query = query.OrderBy(p => p.Group.Name);
                    break;
                default:
                    // Default sort by ID
                    query = query.OrderBy(p => p.ID);
                    break;
            }

            var products = query.Select(p => new ProductResponseDTO
            {
                Id = p.ID,
                Name = p.Name,
                Price = (double)p.Price,
                GroupName = p.Group.Name,
            }).ToList();

            return products;
        }

        private string GetFullGroupName(ProductGroup group)
        {
            if (group == null)
            {
                return string.Empty;
            }

            var groupNames = new List<string>();
            while (group != null)
            {
                var x= _context;
                groupNames.Insert(0, group.Name);
                group = group.PGroup;
            }

            return string.Join(" / ", groupNames);
        }


        public void AddProduct(ProductRequestDTO productDTO)
        {
            if (productDTO == null)
                throw new ArgumentNullException(nameof(productDTO));
            if(!(productDTO.Price > 0))
            {
                throw new ArgumentException("Price must be higher than 0");
            }

            var group = _context.ProductGroups.Find(productDTO.GroupId);
            if (group == null)
                throw new ArgumentException("Invalid group ID.");

            var newProduct = new Product
            {
                Name = productDTO.Name,
                Price = (decimal)productDTO.Price,
                Image = "text",
                GroupID = productDTO.GroupId,
                IsActive = true // Ustawiamy na aktywny domyślnie
            };

            _context.Products.Add(newProduct);
            _context.SaveChanges();
        }

        public void DeactivateProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
                throw new ArgumentException("Product not found.");

            var isNotPayed = _context.Orders
                    .Include(o => o.OrderPositions)
                    .Where(x=>x.IsPayed == false)
                    .SelectMany(x => x.OrderPositions)
                    .Where(x=>x.ProductID == productId).Any();

            if (isNotPayed)
            {
                throw new ArgumentException("This product is in some of not payed order");
            }

            product.IsActive = false;
            _context.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
                throw new ArgumentException("Product not found.");
            var isNotPayed = _context.Orders
                  .Include(o => o.OrderPositions)
                  .Where(x => x.IsPayed == false)
                  .SelectMany(x => x.OrderPositions)
                  .Where(x => x.ProductID == productId).Any();

            var isRelated1 = _context.BasketPositions.Where(x => x.ProductID == productId).Any();
            var isRelated2 = _context.OrderPositions.Where(x => x.ProductID == productId).Any();

            if(isRelated1 || isRelated2)
            {
                DeactivateProduct(productId);
                return;
            }

            if (isNotPayed)
            {
                throw new ArgumentException("This product is in some of not payed order");
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
        public void ActivateProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
                throw new ArgumentException("Product not found.");

            product.IsActive = true;
            _context.SaveChanges();
        }
    }
}
