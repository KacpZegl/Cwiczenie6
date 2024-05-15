using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OrderPosition
    {
        public int ID { get; set; }
        public int OrderID { get; set; }

        public int Amount { get; set; }
        public double Price { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }    
        public Order Order { get; set; }
    }
}
