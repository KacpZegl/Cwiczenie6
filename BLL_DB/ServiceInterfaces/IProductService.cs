using BLL_DB.DTOModels;
using System.Collections.Generic;

namespace BLL_DB.ServiceInterfaces
{
    public interface IProductService
    {
        void ActivateProduct(int productId);
        void AddProduct(ProductRequestDTO productDTO);
        void DeactivateProduct(int productId);
        void DeleteProduct(int productId);
        IEnumerable<ProductResponseDTO> GetProducts(string sortBy, string filterByName, string filterByGroupName, int? filterByGroupId, bool includeInactive);
    }
}
