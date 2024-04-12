using Microsoft.VisualBasic;
using QBFC16Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventorySync
{
    public class ItemDeleter
    {
        public static List<QBItem> Delete(List<QBItem> items)
        {
            //Create the session Manager object
            QBSessionManager sessionManager = new QBSessionManager();

            try
            {

                //Create the message set request object to hold our request
                IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 16, 0);
                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                IListDel ListDelRq = requestMsgSet.AppendListDelRq();
                ListDelRq.ListDelType.SetValue(ENListDelType.ldtItemInventory);

                //Connect to QuickBooks and begin a session
                sessionManager.OpenConnection("", "ItemDeleter");
                sessionManager.BeginSession("", ENOpenMode.omDontCare);

                foreach (QBItem item in items)
                {
                    ListDelRq.ListID.SetValue(item.QBID);
                    //Send the request and get the response from QuickBooks
                    IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);

                    GetResponse(responseMsgSet, item);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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
