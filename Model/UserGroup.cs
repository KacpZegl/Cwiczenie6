using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserGroup
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public IList<User> Users { get; } = new List<User>(); 
    }
}
