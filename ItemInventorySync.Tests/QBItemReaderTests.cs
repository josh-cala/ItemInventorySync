using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventorySync.Tests
{
    public class QBItemReaderTests
    {
        // This test does not work yet
        [Fact]
        public void ReadItems_AddItems_ContainsItems()
        {
            List<QBItem> itemsBeforeAdd = QBItemReader.ReadItems();

            List<IItem> itemsAdded = new List<IItem>();
            itemsAdded.Add(new QBItem("readTest_1", "1", "Materials"));
            itemsAdded.Add(new QBItem("readTest_2", "2", "Materials"));
            itemsAdded.Add(new QBItem("readTest_3", "3", "Materials"));

            itemsAdded = ItemAdder.Add(itemsAdded);

            List<QBItem> itemsAfterAdd = QBItemReader.ReadItems();

            Assert.Equal(itemsBeforeAdd.Count()+3, itemsAfterAdd.Count());

            List<String> ids = QBItemReader.GetQBIDs(itemsAdded);
            List<QBItem> itemsDeleted = new List<QBItem>();

            foreach (string id in ids)
            {
                itemsDeleted.Add(new QBItem(id));
            }
            ItemDeleter.Delete(itemsDeleted);
        }
    }
}
