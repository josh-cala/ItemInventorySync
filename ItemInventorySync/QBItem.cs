using System.Security.Principal;

namespace ItemInventorySync
{
    public class QBItem : IItem
    {
        public string QBID { get; set; }
        public string Rev { get; set; }
        public string Name { get; set; }
        public string ExcelID { get; set; }
        public string IncomeAccount { get; set; }
        public Status Status { get; set; }

        public QBItem(string qbID, string rev, string name, string excelID, string incomeAccount)
        {
            QBID = qbID;
            Rev = rev;
            Name = name;
            ExcelID = excelID;
            IncomeAccount = incomeAccount;
        }

        public QBItem(string name, string excelID, string incomeAccount)
        {
            Name = name;
            ExcelID = excelID;
            IncomeAccount = incomeAccount;
        }

        public QBItem(string qbID)
        {
            QBID = qbID;
        }

        public override string ToString()
        {
            return $"qbID:{QBID}, rev:{Rev}, name:{Name}, excelID:{ExcelID}, type:{IncomeAccount}, Status:{Status}";
        }
    }
}