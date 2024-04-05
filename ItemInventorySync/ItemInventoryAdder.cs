using Microsoft.VisualBasic;
using System.Security.Principal;
using QBFC16Lib;

namespace ItemInventorySync
{
    public class ItemInventoryAdder
    {
        public static List<IItemInventory> Add(List<IItemInventory> itemInventoryList)
        {
            QBSessionManager sessionManager = new QBSessionManager();

            //Connect to QuickBooks and begin a session
            sessionManager.OpenConnection("", "Item Inventory Sync");
            sessionManager.BeginSession("", ENOpenMode.omDontCare);

            foreach (var itemInventory in itemInventoryList)
            {
                try
                {
                    // Create the message set request object to hold our request
                    IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 16, 0);
                    requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                    IItemInventoryAdd ItemInventoryAddRq = requestMsgSet.AppendItemInventoryAddRq();

                    ItemInventoryAddRq.Name.SetValue(itemInventory.Name);
                    ItemInventoryAddRq.COGSAccountRef.FullName.SetValue("Materials");
                    ItemInventoryAddRq.IncomeAccountRef.FullName.SetValue(itemInventory.IncomeAccount);
                    ItemInventoryAddRq.AssetAccountRef.FullName.SetValue("Inventory");
                    ItemInventoryAddRq.ManufacturerPartNumber.SetValue(itemInventory.ExcelID);

                    // Send the request and get the response from QuickBooks
                    IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);

                    // Determine the result
                    if (IsRequestSuccessful(responseMsgSet))
                    {
                        itemInventory.Status = Status.Succeeded;
                    }
                    else
                    {
                        itemInventory.Status = Status.Failed;
                    }
                }
                catch
                {
                    // In case of an exception, mark the term as failed
                    itemInventory.Status = Status.Failed;
                }
            }

            //End the session and close the connection to QuickBooks
            sessionManager.EndSession();
            sessionManager.CloseConnection();
            return itemInventoryList;
        }

        private static bool IsRequestSuccessful(IMsgSetResponse responseMsgSet)
        {
            if (responseMsgSet == null || responseMsgSet.ResponseList == null)
                return false;

            IResponse response = responseMsgSet.ResponseList.GetAt(0);
            return response.StatusCode == 0;
        }
    }

}
