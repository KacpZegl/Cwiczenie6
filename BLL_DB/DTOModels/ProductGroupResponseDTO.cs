using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_DB.DTOModels
{
    public class ProductGroupResponseDTO
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool CanRetrieveSubgroups { get; set; }
    }
}
