using QBFC16Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventorySync
{
    public class ItemUpdater
    {
        public static List<QBItem> Update(List<QBItem> items)
        {
            //Create the session Manager object
            QBSessionManager sessionManager = new QBSessionManager();

            try
            {
                //Create the message set request object to hold our request
                IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 16, 0);
                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                IItemInventoryMod ItemInventoryModRq = requestMsgSet.AppendItemInventoryModRq();

                //Connect to QuickBooks and begin a session
                sessionManager.OpenConnection("", "ItemUpdater");
                sessionManager.BeginSession("", ENOpenMode.omDontCare);

                foreach (QBItem item in items)
                {
                    //Set field value for ListID
                    ItemInventoryModRq.ListID.SetValue(item.QBID);
                    //Set field value for EditSequence
                    ItemInventoryModRq.EditSequence.SetValue(item.Rev);

                    //Update field value for Name
                    ItemInventoryModRq.Name.SetValue(item.Name);

                    /* Optional fields to be updated
                    ItemInventoryModRq.COGSAccountRef.FullName.SetValue("Materials");
                    ItemInventoryModRq.IncomeAccountRef.FullName.SetValue(item.IncomeAccount);
                    ItemInventoryModRq.AssetAccountRef.FullName.SetValue("Inventory");
                    ItemInventoryModRq.ManufacturerPartNumber.SetValue(item.ExcelID);
                    */

                    //Send the request and get the response from QuickBooks
                    IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);

                    GetResponse(responseMsgSet, item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
            finally
            {
                //End the session and close the connection to QuickBooks
                sessionManager.EndSession();
                sessionManager.CloseConnection();
            }

            return items;
        }

        static void GetResponse(IMsgSetResponse responseMsgSet, QBItem item)
        {
            if (responseMsgSet == null) return;
            IResponseList responseList = responseMsgSet.ResponseList;
            if (responseList == null) return;

            IResponse response = responseList.GetAt(0);

            //check the Status code of the response, 0=ok, >0 is warning
            if (response.StatusCode == 0)
            {
                item.Status = Status.Succeeded;
            }
            else
            {
                item.Status = Status.Failed;
            }
        }
    }
}
