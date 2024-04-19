using ItemInventorySync;

List<IItem> itemsAdded = new List<IItem>();
itemsAdded.Add(new QBItem("original name", "1", "Materials"));

itemsAdded = ItemAdder.Add(itemsAdded);
for (int i = 0; i < itemsAdded.Count(); i++)
{
    Console.WriteLine("Added: " + itemsAdded[i]);
}

List<QBItem> itemsUpdated = QBItemReader.GetQBItems(itemsAdded);
itemsUpdated[0].Name = "changed name";

itemsUpdated = ItemUpdater.Update(itemsUpdated);
for (int i = 0; i < itemsUpdated.Count(); i++)
{
    Console.WriteLine("Updated: "+ itemsUpdated[i]);
}

List<QBItem> itemsDeleted = itemsUpdated;

itemsDeleted = ItemDeleter.Delete(itemsDeleted);
for (int i = 0; i < itemsDeleted.Count(); i++)
{
    Console.WriteLine("Deleted: " + itemsDeleted[i]);
}