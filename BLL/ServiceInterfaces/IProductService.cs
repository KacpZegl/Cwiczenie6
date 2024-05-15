using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IProductService
    {
        IEnumerable<ProductResponseDTO> GetProducts(
            string sortBy,
            string filterByName,
            string filterByGroupName,
            int? filterByGroupId,
            bool includeInactive);

        void AddProduct(ProductRequestDTO productDTO);

        void DeactivateProduct(int productId);
        void DeleteProduct(int productId);
        void ActivateProduct(int productId);
    }

}
