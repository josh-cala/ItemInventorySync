using QBFC16Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventorySync.Tests
{
    public class QBItemReaderTests
    {
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

            List<QBItem> itemsDeleted = QBItemReader.GetQBItems(itemsAdded);
            ItemDeleter.Delete(itemsDeleted);

            Assert.Equal(itemsBeforeAdd.Count+3, itemsAfterAdd.Count);
            Assert.Equal(itemsAdded.Count, itemsDeleted.Count);
        }

        [Fact]
        public void GetQBItems_ReadItems_FindItems()
        {
            List<QBItem> itemsRead = QBItemReader.ReadItems();
            List<IItem> itemsToRetrieve = new List<IItem>();

            foreach (QBItem item in itemsRead)
            {
                itemsToRetrieve.Add(item);
            }

            List<QBItem> itemsRetrieved = QBItemReader.GetQBItems(itemsToRetrieve);

            Assert.Equal(itemsToRetrieve.Count, itemsRetrieved.Count);
            for (int i = 0; i < itemsToRetrieve.Count; i++)
            {
                Assert.Equal(itemsToRetrieve[i].Name, itemsRetrieved[i].Name);
            }
        }
    }
}
