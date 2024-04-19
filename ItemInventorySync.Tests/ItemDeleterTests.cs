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

            List<QBItem> itemsDeleted = QBItemReader.GetQBItems(itemsAdded);
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

            List<QBItem> itemsDeleted = QBItemReader.GetQBItems(itemsAdded);
            itemsDeleted = ItemDeleter.Delete(itemsDeleted); // Delete first time

            Assert.Equal(Status.Succeeded, itemsAdded[0].Status);
            Assert.Equal(Status.Succeeded, itemsDeleted[0].Status);

            itemsDeleted = ItemDeleter.Delete(itemsDeleted); // Delete second time

            Assert.Equal(Status.Failed, itemsDeleted[0].Status);
        }

        [Fact]
        public void Delete_AllInvalid_AllFailed()
        {
            List<QBItem> itemsDeleted = new List<QBItem>();
            itemsDeleted.Add(new QBItem("-1"));
            itemsDeleted.Add(new QBItem("-2"));
            itemsDeleted.Add(new QBItem("-3"));

            itemsDeleted = ItemDeleter.Delete(itemsDeleted);

            foreach (QBItem item in itemsDeleted)
            {
                Assert.Equal(Status.Failed, item.Status);
            }
        }
    }
}