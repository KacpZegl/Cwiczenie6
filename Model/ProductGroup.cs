using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductGroup
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? PGroupID { get; set; }
        public ProductGroup? PGroup { get; set; }

   
    }

}
