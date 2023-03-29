using BookIt.Core.Common;
using BookIt.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookIt.Core.Entities
{
    public class Table : BaseEntity
    {
        public string capacity { get; set; }
        
        public TableAreaEnum area { get; set; }

        public TableSmokingEnum smoking { get; set; }

    }
}
