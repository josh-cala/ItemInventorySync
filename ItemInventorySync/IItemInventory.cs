using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventorySync
{
    public interface IItemInventory
    {
        string Name { get; set; }
        string IncomeAccount { get; set; }
        string ExcelID { get; set; }
        Status Status { get; set; }
    }

    public enum Status
    {
        Unprocessed,
        Succeeded,
        Failed
    }
}
