using ItemInventorySync;

List<IItem> itemsAdded = new List<IItem>();
itemsAdded.Add(new QBItem("test add", "62", "Materials"));
ItemAdder.Add(itemsAdded);

List<QBItem> items = QBItemReader.DoPartsQuery();

Console.WriteLine(items.Count);
foreach(QBItem item in items)
{
    Console.WriteLine(item.ToString());
}

List<QBItem> itemsDeleted = new List<QBItem>();
itemsDeleted.Add(items[1]);

ItemDeleter.Delete(itemsDeleted);