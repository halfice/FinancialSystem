using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using Microsoft.SharePoint.Client;
using System.Data;
using System.Net;
using SP = Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Workflow;
using System.IO;
namespace QCC_Market_Survellence
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "NBAD_UtilityPaymentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select NBAD_UtilityPaymentService.svc or NBAD_UtilityPaymentService.svc.cs at the Solution Explorer and start debugging.
    public class NBAD_UtilityPaymentService : INBAD_UtilityPaymentService
    {
        public string DoWork()
        {
            return "Do Work";
        }
        public  string Inquiry(string TradingLicenseNo, string ServiceName)
        {
           // WriteErrorStream(TradingLicenseNo);
            string Result = string.Empty;
            string Caml = string.Empty;
            string idTrade = string.Empty;
            for (int i = 0; i < TradingLicenseNo.Length; i++)
            {
                if (TradingLicenseNo[i].ToString() == "0")
                {
                }
                else
                {
                    idTrade = TradingLicenseNo.Substring(i);
                    break;
                }
            }
            string ChargesTemp = "";
            switch (ServiceName)
            {
                
                case "Scale":
                    Caml = @"<View><Query><Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + idTrade + "</Value></Eq></Where>         </Query><ViewFields><FieldRef Name='VerificationCharges' /><FieldRef Name='ScaleManufacturer' />       <FieldRef Name='CompanyId' /><FieldRef Name='ID' /><FieldRef Name='VerificationCharges' />   </ViewFields></View>";
                    //Caml = string.Format(@"<View><Query><Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>{0}</Value></Eq></Where>         </Query><ViewFields><FieldRef Name='VerificationCharges' /><FieldRef Name='ScaleManufacturer' />       <FieldRef Name='CompanyId' /><FieldRef Name='ID' /><FieldRef Name='VerificationCharges' />   </ViewFields></View>", idTrade);
                    Result = InquirtyDetails("QCCScalesTests", Caml, "VerificationCharges", "VerificationCharges", out ChargesTemp);
                    break;
                case "Fuel":
                    //Caml = string.Format(@"<View><Query><Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>{0}</Value></Eq></Where> </Query> <ViewFields><FieldRef Name='FuelAmount' /></ViewFields></View>", idTrade);
                    Caml = @"<View><Query><Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + idTrade + "</Value></Eq></Where> </Query> <ViewFields><FieldRef Name='FuelAmount' /></ViewFields></View>";
                    Result = InquirtyDetails("FuelTests", Caml, "FuelAmount", "FuelAmount", out ChargesTemp);
                    break;
                case "Consumer Safety":
                    Caml = @"<View><Query> <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + idTrade + "</Value></Eq></Where></Query><ViewFields><FieldRef Name='Charges' /></ViewFields></View>";
                    //Caml = string.Format(@"<View><Query> <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>{0}</Value></Eq></Where></Query><ViewFields><FieldRef Name='Charges' /></ViewFields></View>", idTrade);
                    Result = InquirtyDetails("ConsumerSafetyTest", Caml, "Charges", "Charges", out ChargesTemp);
                    break;
            }
            return Result;
        }
        public  string InquirtyDetails(string ListName, string Caml, string ReturnParamName,string Parameter2, out string Charges)
        {
            string Result = string.Empty;
            Result = "-100";
            
            try
            {
                ClientContext clientContext = new ClientContext("https://apps.qcc.abudhabi.ae/Inspections");
                Microsoft.SharePoint.Client.List spList = clientContext.Web.Lists.GetByTitle(ListName);
                clientContext.Load(spList);
                clientContext.ExecuteQuery();
                if (spList != null && spList.ItemCount > 0)
                {
                    Charges = "0.0";
                    Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = Caml;
                    ListItemCollection listItems = spList.GetItems(camlQuery);
                    clientContext.Load(listItems);
                    clientContext.ExecuteQuery();
                    if ((listItems != null) && (listItems.Count > 0))
                    {
                        foreach (SP.ListItem oListItem in listItems)
                        {
                            Result = Convert.ToString(oListItem[ReturnParamName]);
                            Charges = Convert.ToString(oListItem[Parameter2]);
                            if (Result == "" || Result == "BankTransactionID")
                            {
                                Result = "0";
                                Charges = Convert.ToString(oListItem[Parameter2]);
                            }
                            break;
                        }

                    }
                    else
                    {
                        Charges = "0.0";
                        if (listItems.Count == 0)
                        {
                            Result = "Inavlid Consumer";
                        }

                    }
                }
                else
                {
                    Charges = "0.0";
                }
            }
            catch (Exception ex)
            {
                Result = "-100";
                Charges = "0.0";
                WriteErrorStream("InquirtyDetails",ex.Message.ToString(),"ListName="+ListName +  "Caml="+Caml+ "ReturnParamName="+ReturnParamName + "Parameter2=" +Parameter2+ "Charges="+Charges);
              
                
            }
            return Result;
        }
        public string Payment(string ServiceName, string TradingLicenseNo, string BankTransactionId)
        {
            string Result = string.Empty;
            string Caml = string.Empty;
            string Res = string.Empty;
            string idTrade = string.Empty;
            for (int i = 0; i < TradingLicenseNo.Length; i++)
            {
                if (TradingLicenseNo[i].ToString() == "0")
                {
                }
                else
                {
                    idTrade = TradingLicenseNo.Substring(i);
                    break;
                }
            }
            string ChargesParameter = string.Empty;
            switch (ServiceName)
            {
                case "Scale":
                    Caml = string.Format(@"<View><Query><Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>{0}</Value></Eq></Where></Query><ViewFields><FieldRef Name='BankTransactionID'/><FieldRef Name='VerificationCharges'/></ViewFields></View>", idTrade);
                    ChargesParameter = "";
                    Res = InquirtyDetails("QCCScalesTests", Caml, "BankTransactionID", "VerificationCharges", out ChargesParameter);

                    if ((Res != null) && (Res != string.Empty) && (Res == "0"))
                    {
                        Result = PaymentUtilzer(idTrade, "QCCScalesTests", BankTransactionId);
                    }
                    else
                    {
                        if (Result == "")
                        {
                            if (Res != "" && Res != "-100")
                            {
                                Result = "Reject | Already Paid";
                            }
                            else
                            {
                                if (Res == "-100")
                                {
                                    Result = "Reject | Not Found";
                                }
                                else { Result = "Reject"; }
                            }
                        }
                       
                    }
                    break;
                case "Fuel":
                    //string[] PassValuesFuel = TradingLicenseNo.Split('|');
                    Caml = string.Format(@"<View>    <Query> <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>{0}</Value></Eq></Where> </Query><ViewFields><FieldRef Name='FuelAmount' /><FieldRef Name='BankTransactionID' /></ViewFields>     </View>", idTrade);
                    ChargesParameter = "";
                    Res = InquirtyDetails("FuelTests", Caml, "BankTransactionID", "FuelAmount", out ChargesParameter);
                    if ((Res != null) && (Res != string.Empty) && (Res == "0"))
                    {
                        Result = PaymentUtilzer(idTrade, "FuelTests", BankTransactionId);
                    }
                    else
                    {
                        if (Result == "")
                        {
                            if (Res != "" && Res!="-100")
                            {
                                Result = "Reject | Already Paid";
                            }
                            else
                            {
                                if (Res == "-100")
                                {
                                    Result = "Reject | Not Found";
                                }
                                else { Result = "Reject"; }
                            }
                        }
                        
                    }
                    
                    break;
                case "Consumer Safety":
                    Caml = string.Format(@"<View>    <Query>      <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>{0}</Value></Eq></Where> </Query><ViewFields><FieldRef Name='Charges' /><FieldRef Name='BankTransactionID' /></ViewFields> </View>", idTrade);
                    ChargesParameter = "";
                    Res = InquirtyDetails("ConsumerSafetyTest", Caml, "BankTransactionID", "Charges", out ChargesParameter);
                    if ((Res != null) && (Res != string.Empty) && (Res == "0"))
                    {
                        Result = PaymentUtilzer(idTrade, "ConsumerSafetyTest", BankTransactionId);
                    }
                    else
                    {
                        if (Result == "")
                        {
                            if (Res != "" && Res != "-100")
                            {
                                Result = "Reject | Already Paid";
                            }
                            else
                            {
                                if (Res == "-100")
                                {
                                    Result = "Reject | Not Found";
                                }
                                else { Result = "Reject"; }
                            }
                        }
                       
                    }
                    break;
            }
            return Result;
        }
        public  string PaymentUtilzer(string ID, string ListName, string BankTransactionId)
        {
            string Res = string.Empty;
            try
            {
                DateTime time = DateTime.Now;
                string format = "yyyy-MM-ddTHH:mm:ssZ";
                ;
                var siteUrl = "https://apps.qcc.abudhabi.ae/Inspections/";
                ClientContext clientContext = new ClientContext(siteUrl);
                NetworkCredential credentials = new NetworkCredential("bot1", "12345678", "ADQCC");
                clientContext.Credentials = credentials;
                List oList = clientContext.Web.Lists.GetByTitle(ListName);
                ListItem oListItem = oList.GetItemById(ID);
                oListItem["PaymentReportStatus"] = "PAID";
                oListItem["PaymentUpdateBy"] = "NBAD";
                oListItem["TestStage"] = "1600";
                oListItem["BankTransactionID"] = BankTransactionId;
                oListItem["BankTransactionDate"] = time.ToString(format);
                oListItem.Update();
                clientContext.ExecuteQuery();
                Res = "Accept";
            }
            catch (Exception ex)
            {
                Res = "Reject";
                WriteErrorStream("PaymentUtilzer", ex.Message.ToString(), "ID="+ID+"ListName="+ListName+ "BankTransactionId=" +BankTransactionId);
                //later it should go to sql


            }
            return Res;
        }
        public  List<ADQCCReconcile> Reconcile(string ServiceName, string FromDate, string Todadate)
        {
            List<ADQCCReconcile> Data = new List<ADQCCReconcile>();
            string Caml = string.Empty;
            try
            {
                switch (ServiceName)
                {
                    case "Scale":
                        Caml = string.Format(@"<View>  
                                                    <Query> 
                                                       <Where><And><And><Geq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And><Neq><FieldRef Name='TradingLiecense' /><Value Type='Text'>''</Value></Neq></And></Where> 
                                                    </Query> 
                                                     <ViewFields><FieldRef Name='TestStage' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='TradingLiecense' /><FieldRef Name='VerificationCharges' /><FieldRef Name='BankTransactionDate' /><FieldRef Name='BankTransactionID' /></ViewFields> 
                                              </View>", FromDate, Todadate);
                        Data = InquirtyDetailsReconcile("QCCScalesTests", Caml, "Scale");
                        break;
                    case "Fuel":

                        Caml = string.Format(@"<View>   <Query>           <Where><And><Geq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And></Where> </Query> <ViewFields><FieldRef Name='Title' /><FieldRef Name='Test_StationId' /><FieldRef Name='FuelAmount' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='Test_StationId' /><FieldRef Name='BankTransactionDate' /><FieldRef Name='TestStage' /><FieldRef Name='BankTransactionID' /></ViewFields>       </View>", FromDate, Todadate);
                        Data = InquirtyDetailsReconcile("FuelTests", Caml, "Fuel");
                        break;
                    case "Consumer Safety":

                        Caml = string.Format(@"<View>   <Query>           <Where><And><Geq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And></Where> </Query> <ViewFields><FieldRef Name='Title' /><FieldRef Name='TradeLiecense' /><FieldRef Name='Charges' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='TradeLiecense' /><FieldRef Name='BankTransactionDate' /><FieldRef Name='TestStage' /><FieldRef Name='BankTransactionID' /></ViewFields>       </View>", FromDate, Todadate);
                        Data = InquirtyDetailsReconcile("ConsumerSafetyTest", Caml, "Consumer");

                        break;
                }

            }
            catch (Exception ex)
            {
                // _ReconcileData.Error = ex.Message.ToString();

            }
            return Data;
            //



        }
        public List<ADQCCReconcile> InquirtyDetailsReconcile(string ListName, string Caml, string ServiceName)
        {
            List<ADQCCReconcile> FileList = new List<ADQCCReconcile>();
            string Result = string.Empty;
            Result = "-100";
            try
            {
                ClientContext clientContext = new ClientContext("https://apps.qcc.abudhabi.ae/Inspections");
                Microsoft.SharePoint.Client.List spList = clientContext.Web.Lists.GetByTitle(ListName);
                clientContext.Load(spList);
                clientContext.ExecuteQuery();
                if (spList != null && spList.ItemCount > 0)
                {
                    Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = Caml;

                    ListItemCollection listItems = spList.GetItems(camlQuery);
                    clientContext.Load(listItems);
                    clientContext.ExecuteQuery();
                    if ((listItems != null) && (listItems.Count > 0))
                    {
                        foreach (SP.ListItem oListItem in listItems)
                        {

                            ADQCCReconcile _File = new ADQCCReconcile();
                            string TradeLiecnese = string.Empty;
                            string Charges = string.Empty;
                            string ID = string.Empty;
                            switch (ServiceName)
                            {
                                case "Scale":

                                    _File.Status = Convert.ToString(oListItem["TestStage"]);
                                    TradeLiecnese = Convert.ToString(oListItem["TradingLiecense"]);
                                    Charges = Convert.ToString(oListItem["VerificationCharges"]);
                                    ID = Convert.ToString(oListItem["ID"]);
                                    _File.TradeLiecnese = TradeLiecnese;
                                    _File.Charges = Charges;
                                    //_File.TestIdentifier = TradeLiecnese + "|" + ID + "|" + Charges;
                                    _File.TestDate = Convert.ToString(oListItem["BankTransactionDate"]);
                                    _File.BankTransactionID = Convert.ToString(oListItem["BankTransactionID"]);
                                    
                                    _File.Status = Convert.ToString(oListItem["PaymentReportStatus"]);
                                    _File.BarCodeString = ID.PadLeft(13,'0').ToString(); //TradeLiecnese + "|" + ID + "|" + Charges;
                                    _File.ServiceType = "Scale";
                                    
                                    if (Convert.ToString(oListItem["BankTransactionID"]) != "0")
                                    {
                                        FileList.Add(_File);
                                    }
                                    break;
                                case "Fuel":

                                    _File.Status = Convert.ToString(oListItem["TestStage"]);
                                    TradeLiecnese = Convert.ToString(oListItem["Test_StationId"]);
                                    Charges = Convert.ToString(oListItem["FuelAmount"]);
                                    ID = Convert.ToString(oListItem["ID"]);
                                    _File.TradeLiecnese = TradeLiecnese;
                                    _File.Charges = Charges;
                                    _File.BankTransactionID = Convert.ToString(oListItem["BankTransactionID"]);
                                   // _File.TestIdentifier = TradeLiecnese + "|" + ID + "|" + Charges;
                                    _File.TestDate = Convert.ToString(oListItem["BankTransactionDate"]);
                                    _File.BarCodeString = _File.BarCodeString = ID.PadLeft(13,'0').ToString();//TradeLiecnese + "|" + ID + "|" + Charges;
                                    _File.ServiceType = "Fuel";
                                    if (Convert.ToString(oListItem["BankTransactionID"]) != "BankTransactionID")
                                    {
                                        FileList.Add(_File);
                                    }
                                    break;

                                case "Consumer Safety":
                                    _File.Status = Convert.ToString(oListItem["TestStage"]);
                                    TradeLiecnese = Convert.ToString(oListItem["TradeLiecense"]);
                                    Charges = Convert.ToString(oListItem["Charges"]);
                                    ID = Convert.ToString(oListItem["ID"]);
                                    _File.TradeLiecnese = TradeLiecnese;
                                    _File.Charges = Charges;
                                   // _File.TestIdentifier = TradeLiecnese + "|" + ID + "|" + Charges;
                                    _File.TestDate = Convert.ToString(oListItem["BankTransactionDate"]);
                                    _File.BankTransactionID = Convert.ToString(oListItem["BankTransactionID"]);
                                       _File.BarCodeString =_File.BarCodeString = ID.PadLeft(13,'0').ToString();// TradeLiecnese + "|" + ID + "|" + Charges;
                                       _File.ServiceType = "Consumer";
                                       if (Convert.ToString(oListItem["BankTransactionID"]) != "")
                                       {
                                           FileList.Add(_File);
                                       }
                                    
                                    break;


                            }




                        }

                    }
                }

            }
            catch (Exception ex)
            {

                Result = "-100";
            }
            return FileList;
        }
        public List<ADQCCReconcile>[] ReconcileAll(string Keyword, string FromDate, string Todadate)
        {
            List<ADQCCReconcile> Data = new List<ADQCCReconcile>();
            List<ADQCCReconcile>[] Data1 = new List<ADQCCReconcile>[3];

            string Caml = string.Empty;
            try
            {
//                Caml = string.Format(@"<View>  
//                                                    <Query> 
//                                                       <Where><And><And><Geq><FieldRef Name='Created' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='Created' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And><Neq><FieldRef Name='TradingLiecense' /><Value Type='Text'>''</Value></Neq></And></Where> 
//                                                    </Query> 
//                                                     <ViewFields><FieldRef Name='TestStage' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='TradingLiecense' /><FieldRef Name='VerificationCharges' /><FieldRef Name='BankTransactionID' /><FieldRef Name='BankTransactionID' /></ViewFields> 
//                                              </View>", FromDate, Todadate);
//                Data = InquirtyDetailsReconcile("QCCScalesTests", Caml, "Scale");
//                Data1[0] = Data;


//                Caml = string.Format(@"<View>   <Query>           <Where><And><Geq><FieldRef Name='Created' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='Created' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And></Where> </Query> <ViewFields><FieldRef Name='Title' /><FieldRef Name='Test_StationId' /><FieldRef Name='FuelAmount' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='Test_StationId' /><FieldRef Name='Created' /><FieldRef Name='TestStage' /><FieldRef Name='BankTransactionID' /></ViewFields>       </View>", FromDate, Todadate);
//                Data = InquirtyDetailsReconcile("FuelTests", Caml, "Fuel");
//                Data1[1] = Data;


//                Caml = string.Format(@"<View>   <Query>           <Where><And><Geq><FieldRef Name='Created' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='Created' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And></Where> </Query> <ViewFields><FieldRef Name='Title' /><FieldRef Name='TradeLiecense' /><FieldRef Name='Charges' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='TradeLiecense' /><FieldRef Name='Created' /><FieldRef Name='TestStage' /><FieldRef Name='BankTransactionID' /></ViewFields>       </View>", FromDate, Todadate);
//                Data = InquirtyDetailsReconcile("ConsumerSafetyTest", Caml, "Consumer");
//                Data1[2] = Data;

              //  


                Caml = string.Format(@"<View>  
                                                    <Query> 
                                                       <Where><And><And><Geq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And><Neq><FieldRef Name='TradingLiecense' /><Value Type='Text'>''</Value></Neq></And></Where> 
                                                    </Query> 
                                                     <ViewFields><FieldRef Name='TestStage' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='TradingLiecense' /><FieldRef Name='VerificationCharges' /><FieldRef Name='BankTransactionDate' /><FieldRef Name='BankTransactionID' /></ViewFields> 
                                              </View>", FromDate, Todadate);
                Data = InquirtyDetailsReconcile("QCCScalesTests", Caml, "Scale");
                Data1[0] = Data;


                Caml = string.Format(@"<View>   <Query>           <Where><And><Geq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And></Where> </Query> <ViewFields><FieldRef Name='Title' /><FieldRef Name='Test_StationId' /><FieldRef Name='FuelAmount' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='Test_StationId' /><FieldRef Name='BankTransactionDate' /><FieldRef Name='TestStage' /><FieldRef Name='BankTransactionID' /></ViewFields>       </View>", FromDate, Todadate);
                Data = InquirtyDetailsReconcile("FuelTests", Caml, "Fuel");
                Data1[1] = Data;


                Caml = string.Format(@"<View>   <Query>           <Where><And><Geq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='BankTransactionDate' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And></Where> </Query> <ViewFields><FieldRef Name='Title' /><FieldRef Name='TradeLiecense' /><FieldRef Name='Charges' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='TradeLiecense' /><FieldRef Name='BankTransactionDate' /><FieldRef Name='TestStage' /><FieldRef Name='BankTransactionID' /></ViewFields>       </View>", FromDate, Todadate);
                Data = InquirtyDetailsReconcile("ConsumerSafetyTest", Caml, "Consumer Safety");
                Data1[2] = Data;



            }
            catch (Exception ex)
            {
                // _ReconcileData.Error = ex.Message.ToString();

            }
            return Data1;
            //



        }
        public string Fun()
        
        {
            
            
            return "fun"; 
        
        }
        #region ContactUs
        public void AddContentUsItem(string name, string telephoe, string email, string comment, string res)
        {
            try
            {
                ClientContext clientContext = new ClientContext("http://spappsrv1/English");
                List oList = clientContext.Web.Lists.GetByTitle("ContactUs");
                NetworkCredential credentials = new NetworkCredential("a.frooqi", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;
                ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                ListItem oListItem = oList.AddItem(itemCreateInfo);
                oListItem["Title"] = "QCC Meeting Request Management";
                oListItem["m01a"] = name;
                oListItem["juhk"] = telephoe;
                oListItem["_x0078_fh1"] = email;
                oListItem["a7m7"] = comment;
                oListItem.Update();
                clientContext.ExecuteQuery();
            }
            catch (Exception)
            {

                //throw;
            }
        }
        #endregion
        public string Payments(string ServiceName, string TradingLicenseNo, string BankTransactionId, double Amount)
        {
            string Result = string.Empty;
            string Caml = string.Empty;
            string Res = string.Empty;
            string idTrade = string.Empty;
            for (int i = 0; i < TradingLicenseNo.Length; i++)
            {
                if (TradingLicenseNo[i].ToString() == "0")
                {
                }
                else
                {
                    idTrade = TradingLicenseNo.Substring(i);
                    break;
                }
            }
            string ChargesParameter = string.Empty;
            switch (ServiceName)
            {
                case "Scale":
                    //Caml = string.Format(@"<View>    <Query> <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>{0}</Value></Eq></Where> </Query><ViewFields><FieldRef Name='BankTransactionID' /><FieldRef Name='VerificationCharges' /></ViewFields>   </View>", idTrade);
                    Caml = @"<View>    <Query> <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>"+idTrade+"</Value></Eq></Where> </Query><ViewFields><FieldRef Name='BankTransactionID' /><FieldRef Name='VerificationCharges' /></ViewFields>   </View>";
                    Res = InquirtyDetails("QCCScalesTests", Caml, "BankTransactionID", "VerificationCharges", out ChargesParameter);
                    if ((Res != null) && (Res != string.Empty) && (Res == "0"))
                    {
                        double AmountInSystem = Convert.ToDouble(ChargesParameter);
                        if (AmountInSystem == Amount)
                        {
                            Result = PaymentUtilzer(idTrade, "QCCScalesTests", BankTransactionId);
                        }
                        else
                        {
                            Result = "Reject | Not allowed to pay Invalid Amount";
                        }
                    }
                    else
                    {
                        if (Result == "")
                        {
                            if (Res != "" && Res != "-100")
                            {
                                if (Res == "Inavlid Consumer")
                                {
                                    Result = Res;
                                }
                                else
                                {
                                    Result = "Reject | Already Paid";
                                }
                            }
                            else
                            {
                                if (Res == "-100")
                                {
                                    Result = "Reject | Not Found";
                                }
                                else { Result = "Reject"; }
                            }
                        }

                    }
                    break;
                case "Fuel":
                    //string[] PassValuesFuel = TradingLicenseNo.Split('|');
                    //Caml = string.Format(@"<View>    <Query> <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>{0}</Value></Eq></Where> </Query><ViewFields><FieldRef Name='FuelAmount' /><FieldRef Name='BankTransactionID' /></ViewFields>     </View>", idTrade);
                    Caml = "<View><Query><Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + idTrade + "</Value></Eq></Where></Query><ViewFields><FieldRef Name='FuelAmount' /><FieldRef Name='BankTransactionID' /></ViewFields></View>"; 
                    Res = InquirtyDetails("FuelTests", Caml, "BankTransactionID", "FuelAmount", out ChargesParameter);
                    if ((Res != null) && (Res != string.Empty) && (Res == "0"))
                    {

                        double AmountInSystem = Convert.ToDouble(ChargesParameter);
                        if (AmountInSystem == Amount)
                        {
                            Result = PaymentUtilzer(idTrade, "FuelTests", BankTransactionId);
                        }
                        else
                        {
                            Result = "Reject | Not allowed to pay Invalid Amount";
                        }
                    }
                    else
                    {
                        if (Result == "")
                        {
                            if (Res != "" && Res != "-100")
                            {
                                if (Res == "Inavlid Consumer")
                                {
                                    Result = Res;
                                }
                                else
                                {
                                    Result = "Reject | Already Paid";
                                }
                            }
                            else
                            {
                                if (Res == "-100")
                                {
                                    Result = "Reject | Not Found";
                                }
                                else { Result = "Reject"; }
                            }
                        }

                    }

                    break;
                case "Consumer Safety":
                    //Caml = string.Format(@"<View>    <Query>      <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>{0}</Value></Eq></Where> </Query><ViewFields><FieldRef Name='BankTransactionID' /><ViewFields><FieldRef Name='Charges' /></ViewFields> </View>", idTrade);
                    Caml = "<View><Query><Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + idTrade + "</Value></Eq></Where></Query><ViewFields><FieldRef Name='Charges' /><FieldRef Name='BankTransactionID' /></ViewFields></View>"; 
                    Res = InquirtyDetails("ConsumerSafetyTest", Caml, "BankTransactionID", "Charges", out ChargesParameter);
                    if ((Res != null) && (Res != string.Empty) && (Res == "0"))
                    {
                        double AmountInSystem = Convert.ToDouble(ChargesParameter);
                        if (AmountInSystem == Amount)
                        {
                            Result = PaymentUtilzer(idTrade, "ConsumerSafetyTest", BankTransactionId);
                        }
                        else
                        {
                            Result = "Reject | Not allowed to pay Invalid Amount";
                        }
                    }
                    else
                    {
                        if (Result == "")
                        {
                            if (Res != "" && Res != "-100")
                            {
                                if (Res == "Inavlid Consumer")
                                {
                                    Result = Res;
                                }
                                else
                                {
                                    Result = "Reject | Already Paid";
                                }
                            }
                            else
                            {
                                if (Res == "-100")
                                {
                                    Result = "Reject | Not Found";
                                }
                                else { Result = "Reject"; }
                            }
                        }

                    }
                    break;
            }
            return Result;
        }
        public void WriteErrorStream(string Method, string StreamError, string Paramerters)
        {
            try
            {
               //  string DateSpecifier = DateTime.Now.ToShortDateString().Replace("\\", "-").ToString();
              //  DateSpecifier = DateSpecifier.Replace("/", "-");
               // StreamWriter _ErrorStream = new StreamWriter(@"C:\NBAD\LOGS\" + "NBADLOGS" + DateSpecifier+".txt",true);
               // _ErrorStream.WriteLine("****************************************************");
               // _ErrorStream.WriteLine(Method + ":"+ StreamError +":" +Paramerters);
               // _ErrorStream.WriteLine("****************************************************");

                //_ErrorStream.Close();
               // _ErrorStream.Dispose();


                string url = "";
                url = "https://apps.qcc.abudhabi.ae/Inspections";
                ClientContext clientContext = new ClientContext(url);
                NetworkCredential credentials = new NetworkCredential("bot1", "12345678", "ADQCC");
                clientContext.Credentials = credentials;
                List oList = clientContext.Web.Lists.GetByTitle("NBAD");
                ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                ListItem oListItem = oList.AddItem(itemCreateInfo);
                oListItem["Title"] = "NBAD ONLINE TRANSACTIONS";
                oListItem["Exceptions"] = StreamError;// _obj.invoice_number; ;//invoice_number
                oListItem["MethodName"] = StreamError;// _obj.amount;//amount
                oListItem["Parameters"] = Paramerters;// _obj.amount;//total_amount
                oListItem.Update();
                clientContext.ExecuteQuery();





















            }
            catch (Exception EX)
            {
                
//                throw;
            }
        }
    }
}
/*
 * 
 * 
            string Result = string.Empty;
            string Caml = string.Empty;
            switch (ServiceName)
            {
                case "Scale":
                    string[] PassValues = TradingLicenseNo.Split('|');

                    Caml = string.Format(@"<View>    <Query> 
                                        <Where><And><And><Eq><FieldRef Name='CompanyId' /><Value Type='Text'>{0}</Value></Eq>
                                        <Eq><FieldRef Name='VerificationCharges' /><Value Type='Text'>{1}</Value></Eq></And>
                                        <Eq><FieldRef Name='ID' /><Value Type='Counter'>{2}</Value></Eq>
                                         </And></Where> </Query><ViewFields><FieldRef Name='VerificationCharges' /></ViewFields> 
                        </View>", PassValues[0], PassValues[2], PassValues[1]);
                    //  Result = Caml;
                    Result = PaymentUtilzer(PassValues[1], "QCCScalesTests", BankTransactionId);
                    break;
                case "Fuel":
                    string[] PassValuesFuel = TradingLicenseNo.Split('|');
                    Caml = string.Format(@"<View>  
                                        <Query> 
                                        <Where><Eq><FieldRef Name='ID' /><Value Type='Text'>{1}</Value></Eq></Where> 
                                            </Query> 
                                        <ViewFields><FieldRef Name='FuelAmount' /></ViewFields> 
                    </View>", PassValuesFuel[0], PassValuesFuel[1], PassValuesFuel[2]);
                    Result = PaymentUtilzer(PassValuesFuel[1], "FuelTests", BankTransactionId);
                    break;
                case "Consumer":
                    string[] PassValuesCS = TradingLicenseNo.Split('|');
                    Caml = string.Format(@"<View>  
                                    <Query> 
                                       <Where><Eq><FieldRef Name='ID' /><Value Type='Text'>{1}</Value></Eq></Where> 
                                    </Query> 
                                     <ViewFields><FieldRef Name='Charges' /></ViewFields> 
                              </View>", PassValuesCS[0], PassValuesCS[1], PassValuesCS[2]);
                    Result = PaymentUtilzer(PassValuesCS[1], "ConsumerSafetyTest", BankTransactionId);
                    break;
            }

*/

  
