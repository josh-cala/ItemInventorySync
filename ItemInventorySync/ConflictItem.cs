using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventorySync
{
    public class ConflictItem
    {
        public IItem excelItem { get; set; }
        public IItem qbItem { get; set; }
    }
}
