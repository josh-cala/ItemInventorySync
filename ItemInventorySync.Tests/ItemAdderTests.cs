using ItemInventorySync;
using System.Security.Principal;

namespace ItemInventorySync.Tests
{
    public class ItemAdderTests
    {
        [Fact]
        public void Add_SingleItemInventory_Success()
        {
            IItem itemInventory  = new QBItem("TEST_Single", "1", "Materials");
            var result = ItemAdder.Add(new List<IItem> { itemInventory });

            List<String> ids = QBItemReader.GetQBIDs(result);

            List<QBItem> itemsDeleted = new List<QBItem>();
            foreach (string id in ids)
            {
                itemsDeleted.Add(new QBItem(id));
            }
            ItemDeleter.Delete(itemsDeleted);

            // Verify that the item was added successfully
            Assert.Single(result);
            Assert.Equal(Status.Succeeded, result[0].Status);
        }

        [Fact]
        public void Add_MultItems_Success()
        {
            IItem inv1 = new QBItem("TEST_MultOne", "101", "Bank Service Charges");
            IItem inv2 = new QBItem("TEST_MultTwo", "102", "Meals and Entertainment");
            IItem inv3 = new QBItem("TEST_MultThree", "103", "Freight and Shipping Costs");

            var result = ItemAdder.Add(new List<IItem> { inv1, inv2, inv3 });

            List<String> ids = QBItemReader.GetQBIDs(result);

            List<QBItem> itemsDeleted = new List<QBItem>();
            foreach (string id in ids)
            {
                itemsDeleted.Add(new QBItem(id));
            }
            ItemDeleter.Delete(itemsDeleted);

            // Verify that each item was added successfully
            Assert.Equal(3, result.Count);
            foreach (var check in result)
            {
                Assert.Equal(Status.Succeeded, check.Status);
            }
        }
        [Fact]
        public void Add_EdgeCase_OnlyMaxCharacters()
        {
            IItem inv1 = new QBItem("TEST_StringLength31Character", "45456", "Utilities");
            IItem inv2 = new QBItem("TEST_StringLength_BeyondMaxAllowed", "12345678901234567890123456789012", "Utilities");
            IItem inv3 = new QBItem("TEST_EdgeCase", "1234567890123456789012345678901", "Utilities");

            var result = ItemAdder.Add(new List<IItem> { inv1, inv2, inv3 });

            List<String> ids = QBItemReader.GetQBIDs(result);

            List<QBItem> itemsDeleted = new List<QBItem>();
            foreach (string id in ids)
            {
                itemsDeleted.Add(new QBItem(id));
            }
            ItemDeleter.Delete(itemsDeleted);

            // Verify that each item was added successfully
            Assert.Equal(3, result.Count);
            Assert.Equal(Status.Succeeded, result[0].Status);
            Assert.Equal(Status.Failed, result[1].Status);
            Assert.Equal(Status.Succeeded, result[2].Status);
        }
        [Fact]
        public void Add_TwoSameName_Failure()
        {
            IItem inv1 = new QBItem("TEST_Bank", "55", "Telephone Expense");
            IItem inv2 = new QBItem("TEST_Bank", "56", "Payroll Expenses");
            var result = ItemAdder.Add(new List<IItem> { inv1, inv2 });

            List<String> ids = QBItemReader.GetQBIDs(result);

            List<QBItem> itemsDeleted = new List<QBItem>();
            foreach (string id in ids)
            {
                itemsDeleted.Add(new QBItem(id));
            }
            ItemDeleter.Delete(itemsDeleted);

            // Verify that the item was added successfully
            Assert.Equal(2, result.Count);
            Assert.Equal(Status.Succeeded, result[0].Status);
            Assert.Equal(Status.Failed, result[1].Status);
        }

        [Fact]
        public void Add_TypeExist_Failure()
        {
            IItem inv1 = new QBItem("TEST_TypeNull", "8", "");
            IItem inv2 = new QBItem("TEST_TypeNot", "6", "not a valid item");
            var result = ItemAdder.Add(new List<IItem> { inv1 ,inv2 });

            List<String> ids = QBItemReader.GetQBIDs(result);

            List<QBItem> itemsDeleted = new List<QBItem>();
            foreach (string id in ids)
            {
                itemsDeleted.Add(new QBItem(id));
            }
            ItemDeleter.Delete(itemsDeleted);

            // Verify that the item was added successfully
            Assert.Equal(2, result.Count);
            Assert.Equal(Status.Failed, result[0].Status);
            Assert.Equal(Status.Failed, result[1].Status);
        }

        [Fact]
        public void Add_TypeExpense_Failure()
        {
            // Inventory made for purchases should only have income items not expense items

            IItem inv1 = new QBItem("TEST_TypeExpense1", "456", "Items Receivable");
            IItem inv2 = new QBItem("TEST_TypeExpense2", "567", "Items Payable");
            IItem inv3 = new QBItem("TEST_TypeExpense3", "789", "Purchase Orders");
            var result = ItemAdder.Add(new List<IItem> { inv1, inv2, inv3 });

            List<String> ids = QBItemReader.GetQBIDs(result);

            List<QBItem> itemsDeleted = new List<QBItem>();
            foreach (string id in ids)
            {
                itemsDeleted.Add(new QBItem(id));
            }
            ItemDeleter.Delete(itemsDeleted);

            // Verify that the item failed to add
            Assert.Equal(3, result.Count);
            Assert.Equal(Status.Failed, result[0].Status);
            Assert.Equal(Status.Failed, result[1].Status);
            Assert.Equal(Status.Failed, result[2].Status);
        }
    }
}