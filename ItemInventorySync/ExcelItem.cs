using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventorySync
{
    public class ExcelItem : IItem
    {
        public string Name { get; set; }
        public string ExcelID { get; set; }
        public string IncomeAccount { get; set; }
        public Status Status { get; set; }

        public ExcelItem(string name, string excelID, string incomeAccount)
        {
            Name = name;
            ExcelID = excelID;
            IncomeAccount = incomeAccount;
        }
    }
}
