﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public class ProductGroupRequestDTO
    {
        public string GroupName { get; set; }
        public int? ParentGroupId { get; set; }
    }
}
