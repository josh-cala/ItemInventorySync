using Microsoft.VisualBasic;
using QBFC16Lib;
using System.Security.Principal;

namespace ItemInventorySync
{
    public class QBItemReader
    {
        public static List<QBItem> ReadItems()
        {
            //Create the session Manager object
            QBSessionManager sessionManager = new QBSessionManager();

            //Create the message set request object to hold our request
            IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 16, 0);
            requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

            IItemInventoryQuery PartQueryRq = requestMsgSet.AppendItemInventoryQueryRq();

            //Connect to QuickBooks and begin a session
            sessionManager.OpenConnection("", "ItemReader");
            sessionManager.BeginSession("", ENOpenMode.omDontCare);

            //Send the request and get the response from QuickBooks
            IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);

            //End the session and close the connection to QuickBooks
            sessionManager.EndSession();
            sessionManager.CloseConnection();

            return WalkPartsQueryRs(responseMsgSet);
        }


        static List<QBItem> WalkPartsQueryRs(IMsgSetResponse responseMsgSet)
        {
            if (responseMsgSet == null) return null;
            IResponseList responseList = responseMsgSet.ResponseList;
            if (responseList == null) return null;
            //if we sent only one request, there is only one response, we'll walk the list for this sample
            for (int i = 0; i < responseList.Count; i++)
            {
                IResponse response = responseList.GetAt(i);
                //check the Status code of the response, 0=ok, >0 is warning
                if (response.StatusCode >= 0)
                {
                    //the request-specific response is in the details, make sure we have some
                    if (response.Detail != null)
                    {
                        //make sure the response is the type we're expecting
                        ENResponseType responseType = (ENResponseType)response.Type.GetValue();
                        if (responseType == ENResponseType.rtItemInventoryQueryRs)
                        {
                            //upcast to more specific type here, this is safe because we checked with response.Type check above
                            IItemInventoryRetList itemsList = (IItemInventoryRetList)response.Detail;
                            return WalkPartsList(itemsList);
                        }
                    }
                }
            }
            return null;
        }




        static List<QBItem> WalkPartsList(IItemInventoryRetList itemsList)
        {
            if (itemsList == null) return null;
            List<QBItem> items = new List<QBItem>();
            //Go through all the elements of IPartRetList

            for (int i = 0; i < itemsList.Count; i++)
            {
                var item = itemsList.GetAt(i);

                //Get value of ListID
                string qbID = (string)item.ListID.GetValue();

                //Get value of EditSequence
                string rev = (string)item.EditSequence.GetValue();

                //Get value of Name
                string name = (string)item.Name.GetValue();

                //Get value of ManufacturerPartNumber
                string excelID = "-1";
                if(item.ManufacturerPartNumber != null)
                {
                    excelID = (string)item.ManufacturerPartNumber.GetValue();
                }

                //Get value of Income Item FullName
                string incomeAccount = (string)item.IncomeAccountRef.FullName.GetValue();

                items.Add(new QBItem(qbID, rev, name, excelID, incomeAccount));

            }
            return items;
        }

        // Accepts a list of IItems and uses their Names to return a list of corresponding QBItems
        public static List<QBItem> GetQBItems(List<IItem> items)
        {
            List<QBItem> allItems = ReadItems();
            List<QBItem> itemList = new List<QBItem>();

            // Loop through the list of given IItems
            for (int i = 0; i < items.Count; i++)
            {
                // Match the current Item Name to an Item Name found within QuickBooks, then add the QBTerm version to itemList
                QBItem? itemMatch = allItems.Find(x => x.Name == items[i].Name);
                if (itemMatch != null)
                {
                    itemList.Add(itemMatch);
                }
            }

            return itemList;
        }




    }
}
