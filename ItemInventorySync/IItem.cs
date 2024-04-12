using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventorySync
{
    public interface IItem
    {
        string Name { get; set; }
        string ExcelID { get; set; }
        string IncomeAccount { get; set; }
        Status Status { get; set; }
    }

    public enum Status
    {
        Unprocessed,
        Succeeded,
        Failed
    }
}
