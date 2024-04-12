using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventorySync.Tests
{
    public class ItemDeleterTests
    {
        [Fact]
        public void Delete_InNull_OutNull()
        {
            Assert.Null(ItemDeleter.Delete(null));
        }

        [Fact]
        public void Delete_AllValid_AllSucceeded()
        {
            List<IItem> itemsAdded = new List<IItem>();
            itemsAdded.Add(new QBItem("test 1", "1", "Materials"));
            itemsAdded.Add(new QBItem("test 2", "2", "Materials"));
            itemsAdded.Add(new QBItem("test 3", "3", "Materials"));

            itemsAdded = ItemAdder.Add(itemsAdded);

            List<String> ids = QBItemReader.GetQBIDs(itemsAdded);
            List<QBItem> itemsDeleted = new List<QBItem>();

            foreach (string id in ids)
            {
                itemsDeleted.Add(new QBItem(id));
            }

            itemsDeleted = ItemDeleter.Delete(itemsDeleted);

            Assert.Equal(itemsAdded.Count(), itemsDeleted.Count());

            for (int i = 0; i < itemsAdded.Count(); i++)
            {
                Assert.Equal(Status.Succeeded, itemsAdded[i].Status);
                Assert.Equal(Status.Succeeded, itemsDeleted[i].Status);
            }
        }

        [Fact]
        public void Delete_DeleteTwice_1Success1Fail()
        {
            List<IItem> itemsAdded = new List<IItem>();
            itemsAdded.Add(new QBItem("test 4", "4", "Materials"));

            itemsAdded = ItemAdder.Add(itemsAdded);

            List<String> ids = QBItemReader.GetQBIDs(itemsAdded);

            List<QBItem> itemsDeleted = new List<QBItem>();
            foreach (string id in ids)
            {
                itemsDeleted.Add(new QBItem(id));
                itemsDeleted.Add(new QBItem(id));
            }

            itemsDeleted = ItemDeleter.Delete(itemsDeleted);

            Assert.Equal(Status.Succeeded, itemsAdded[0].Status);
            Assert.Equal(Status.Succeeded, itemsDeleted[0].Status);
            Assert.Equal(Status.Failed, itemsDeleted[1].Status);
        }

        [Fact]
        public void Delete_AllInvalid_AllFailed()
        {
            List<QBItem> items = new List<QBItem>();
            items.Add(new QBItem("-1"));
            items.Add(new QBItem("-2"));
            items.Add(new QBItem("-3"));

            items = ItemDeleter.Delete(items);

            foreach (QBItem item in items)
            {
                Assert.Equal(Status.Failed, item.Status);
            }
        }
    }
}