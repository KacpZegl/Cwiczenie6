using BLL_DB.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_DB.ServiceInterfaces
{
    public interface IProductGroupService
    {
        IEnumerable<ProductGroupResponseDTO> GetProductGroups(int? parentId);
        void AddProductGroup(ProductGroupRequestDTO productGroup);
    }
}
