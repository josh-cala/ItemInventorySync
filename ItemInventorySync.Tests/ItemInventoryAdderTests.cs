using ItemInventorySync;
using System.Security.Principal;

namespace ItemInventorySyncTest
{
    public class QBTerm : IItemInventory
    {
        public string Name { get; set; }
        public string IncomeAccount { get; set; }
        public string ExcelID { get; set; }
        public Status Status { get; set; }

        public QBTerm(string name, string incomeAccount, string excelID)
        {
            Name = name;
            IncomeAccount = incomeAccount;
            ExcelID = excelID;
        }

        public override string ToString()
        {
            return $"name:{Name}, accountType: {IncomeAccount}, bid:{ExcelID}";
        }
    }

    public class ItemInventoryAdderTests
    {
        [Fact]
        public void Add_SingleItemInventory_Success()
        {
            IItemInventory itemInventory  = new QBTerm("TEST_Single", "Materials", "1");
            var result = ItemInventoryAdder.Add(new List<IItemInventory > { itemInventory });
            // Verify that the term was added successfully
            Assert.Single(result);
            Assert.Equal(Status.Succeeded, result[0].Status);
        }

        [Fact]
        public void Add_MultTerm_Success()
        {
            IItemInventory inv1 = new QBTerm("TEST_MultOne", "Bank Service Charges", "101");
            IItemInventory inv2 = new QBTerm("TEST_MultTwo", "Meals and Entertainment", "102");
            IItemInventory inv3 = new QBTerm("TEST_MultThree", "Freight and Shipping Costs", "103");

            var result = ItemInventoryAdder.Add(new List<IItemInventory> { inv1, inv2, inv3 });
            // Verify that each term was added successfully
            Assert.Equal(3, result.Count);
            foreach (var check in result)
            {
                Assert.Equal(Status.Succeeded, check.Status);
            }
        }
        [Fact]
        public void Add_EdgeCase_OnlyMaxCharacters()
        {
            IItemInventory inv1 = new QBTerm("TEST_StringLength31Character", "Utilities", "45456");
            IItemInventory inv2 = new QBTerm("TEST_StringLength_BeyondMaxAllowed", "Utilities", "12345678901234567890123456789012");
            IItemInventory inv3 = new QBTerm("TEST_EdgeCase", "Utilities", "1234567890123456789012345678901");

            var result = ItemInventoryAdder.Add(new List<IItemInventory> { inv1, inv2, inv3 });
            // Verify that each term was added successfully
            Assert.Equal(3, result.Count);
            Assert.Equal(Status.Succeeded, result[0].Status);
            Assert.Equal(Status.Failed, result[1].Status);
            Assert.Equal(Status.Succeeded, result[2].Status);
        }
        [Fact]
        public void Add_TwoSameName_Failure()
        {
            IItemInventory inv1 = new QBTerm("TEST_Bank", "Telephone Expense", "55");
            IItemInventory inv2 = new QBTerm("TEST_Bank", "Payroll Expenses", "56");
            var result = ItemInventoryAdder.Add(new List<IItemInventory> { inv1, inv2 });
            // Verify that the term was added successfully
            Assert.Equal(2, result.Count);
            Assert.Equal(Status.Succeeded, result[0].Status);
            Assert.Equal(Status.Failed, result[1].Status);
        }

        [Fact]
        public void Add_TypeExist_Failure()
        {
            IItemInventory inv1 = new QBTerm("TEST_TypeNull", "", "8");
            IItemInventory inv2 = new QBTerm("TEST_TypeNot", "not a valid account", "6");
            var result = ItemInventoryAdder.Add(new List<IItemInventory> { inv1 ,inv2 });
            // Verify that the term was added successfully
            Assert.Equal(2, result.Count);
            Assert.Equal(Status.Failed, result[0].Status);
            Assert.Equal(Status.Failed, result[1].Status);
        }

        [Fact]
        public void Add_TypeExpense_Failure()
        {
            // Inventory made for purchases should only have income accounts not expense accounts

            IItemInventory inv1 = new QBTerm("TEST_TypeExpense1", "Accounts Receivable", "456");
            IItemInventory inv2 = new QBTerm("TEST_TypeExpense2", "Accounts Payable", "567");
            IItemInventory inv3 = new QBTerm("TEST_TypeExpense3", "Purchase Orders", "789");
            var result = ItemInventoryAdder.Add(new List<IItemInventory> { inv1, inv2, inv3 });
            // Verify that the term was added successfully
            Assert.Equal(3, result.Count);
            Assert.Equal(Status.Failed, result[0].Status);
            Assert.Equal(Status.Failed, result[1].Status);
            Assert.Equal(Status.Failed, result[2].Status);
        }
    }
}