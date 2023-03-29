using BookIt.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookIt.Core.Entities
{
    public class TableReservation : BaseEntity
    {
        public Guid TableId { get; set; }
        public Guid DepartmentId { get; set; }

    }
}
