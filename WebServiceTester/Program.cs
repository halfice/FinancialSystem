using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Data;
using System.Net;
using SP = Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Workflow;

namespace WebServiceTester
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine(Inquiry("901136|324|500", "Scale"));
            Console.WriteLine(Payment("901136|324|500", "Scale","34"));
            //Inquiry("701136-321-300", "Scale");
          //  Payment("701136-321-300", "Scale");
           
            Console.WriteLine(Inquiry("701136|321|300", "Scale"));
            Console.WriteLine(Inquiry("CN-8879766|46|100", "Consumer"));
            Console.WriteLine(Inquiry("CN-8879766|46|100", "Consumer"));
            Console.WriteLine(Inquiry("CN-889799|47|100", "Consumer"));
            Console.WriteLine(Inquiry("CN-9987|48|100", "Consumer"));
            Console.WriteLine(Inquiry("CN-111|49|100", "Consumer"));
            Console.WriteLine(Inquiry("CN-111|50|100", "Consumer"));
            Console.WriteLine(Inquiry("TL1|56|100", "Consumer"));
            Console.WriteLine(Inquiry("AD|57|100", "Consumer"));
            Console.WriteLine(Inquiry("tl|58|100", "Consumer"));
            Console.WriteLine(Inquiry("MYtl|60|100", "Consumer"));
            Console.WriteLine(Inquiry("TRL|63|100", "Consumer"));

            Console.WriteLine(Inquiry("901136|324|500", "Scale"));
            Console.WriteLine(Inquiry("901136|326|500", "Scale"));
            Console.WriteLine(Inquiry("35870|327|500", "Scale"));
            Console.WriteLine(Inquiry("35871|328|500", "Scale"));
            Console.WriteLine(Inquiry("05586095058|329|500", "Scale"));
            Console.WriteLine(Inquiry("78|330|500", "Scale"));
            Console.WriteLine(Inquiry("78|331|500", "Scale"));
            Console.WriteLine(Inquiry("78|332|500", "Scale"));
            Console.WriteLine(Inquiry("78|333|500", "Scale"));
            Console.WriteLine(Inquiry("865|0512102|150", "Fuel"));
            Console.WriteLine(Inquiry("866|0512102|150", "Fuel"));
            Console.WriteLine(Inquiry("867|0512102|150", "Fuel"));
            //Console.WriteLine(Payment("78|333|500","Scale"));
            Console.WriteLine(Inquiry("901136|324|500", "Scale"));
            Console.WriteLine(Inquiry("901136|326|500", "Scale"));
            Console.WriteLine(Inquiry("35870|327|500", "Scale"));
            Console.WriteLine(Inquiry("35871|328|500", "Scale"));
            Console.WriteLine(Inquiry("05586095058|329|500", "Scale"));
            Console.WriteLine(Inquiry("78|330|500", "Scale"));
            Console.WriteLine(Inquiry("78|331|500", "Scale"));
            Console.WriteLine(Inquiry("78|332|500", "Scale"));
            Console.WriteLine(Inquiry("78|333|500", "Scale"));



            Console.WriteLine(Inquiry("901136|324|500", "Scale"));
            Console.WriteLine(Inquiry("901136|326|500", "Scale"));
            Console.WriteLine(Inquiry("35870|327|500", "Scale"));
            Console.WriteLine(Inquiry("35871|328|500", "Scale"));
            Console.WriteLine(Inquiry("05586095058|329|500", "Scale"));
            Console.WriteLine(Inquiry("78|330|500", "Scale"));
            Console.WriteLine(Inquiry("78|331|500", "Scale"));
            Console.WriteLine(Inquiry("78|332|500", "Scale"));
            Console.WriteLine(Inquiry("78|333|500", "Scale"));
            Console.WriteLine(Inquiry("901136|324|500", "Scale"));
            Console.WriteLine(Inquiry("901136|326|500", "Scale"));
            Console.WriteLine(Inquiry("35870|327|500", "Scale"));
            Console.WriteLine(Inquiry("35871|328|500", "Scale"));
            Console.WriteLine(Inquiry("05586095058|329|500", "Scale"));
            Console.WriteLine(Inquiry("78|330|500", "Scale"));
            Console.WriteLine(Inquiry("78|331|500", "Scale"));
            Console.WriteLine(Inquiry("78|332|500", "Scale"));
            Console.WriteLine(Inquiry("78|333|500", "Scale"));
            Console.WriteLine(Inquiry("901136|324|500", "Scale"));
            Console.WriteLine(Inquiry("901136|326|500", "Scale"));
            Console.WriteLine(Inquiry("35870|327|500", "Scale"));
            Console.WriteLine(Inquiry("35871|328|500", "Scale"));
            Console.WriteLine(Inquiry("05586095058|329|500", "Scale"));
            Console.WriteLine(Inquiry("78|330|500", "Scale"));
            Console.WriteLine(Inquiry("78|331|500", "Scale"));
            Console.WriteLine(Inquiry("78|332|500", "Scale"));
            Console.WriteLine(Inquiry("78|333|500", "Scale"));
            string Caml = string.Format(@"<View>  
                                                    <Query> 
                                                       <Where><And><And><Geq><FieldRef Name='Created' /><Value Type='DateTime'>2014-03-1800:00:00Z</Value></Geq><Leq><FieldRef Name='Created' /><Value Type='DateTime'>2014-03-1900:00:00Z</Value></Leq></And><Neq><FieldRef Name='TradingLiecense' /><Value Type='Text'>''</Value></Neq></And></Where> 
                                                    </Query> 
                                                     <ViewFields><FieldRef Name='TestStage' /><FieldRef Name='FinalTestResults' /><FieldRef Name='TradingLiecense' /><FieldRef Name='VerificationCharges' /><FieldRef Name='BankTransactionID' /></ViewFields> 
                                              </View>", "", "");
            F = InquirtyDetails("QCCScalesTests", Caml);
             */


            string _REsult = Inquiry("901136|324|500", "Scale");
            string _REsult1 =Payment("Scale", "901136|324|500", "34343");
             List<ADQCCReconcile> F = new List<ADQCCReconcile>();
          //   F = Reconcile("Scale", "2014-01-01", "2014-03-19");
          //// 
          //   for (int i = 0; i < F.Count; i++)
          //   {
          //       //Console.WriteLine("Test Date " + F[i].TestDate + "  Test Serial  " + F[i].TestIdentifier + "  Charges" + F[i].Charges + "Payment Status" + F[i].Status + "Bank Transaction Id" + F[i].BankTransactionID) ;
                
          //   }

          //   List<ADQCCReconcile>[] Ar = new List<ADQCCReconcile>[1];
          //   Ar=ReconcileAll("", "2010-01-01", "2014-03-19");

            Console.ReadLine();
             
            
        }

        public static string Inquiry(string TradingLicenseNo, string ServiceName)
        {
            string Result = string.Empty;
            string Caml = string.Empty;
            //Result = TradingLicenseNo + ServiceName.ToString();

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
                    Result = InquirtyDetails("QCCScalesTests", Caml, "VerificationCharges");
                    break;
                case "Fuel":
                    string[] PassValuesFuel = TradingLicenseNo.Split('|');
                    Caml = string.Format(@"<View>  
                                        <Query> 
                                        <Where><Eq><FieldRef Name='ID' /><Value Type='Text'>{1}</Value></Eq></Where> 
                                            </Query> 
                                        <ViewFields><FieldRef Name='FuelAmount' /></ViewFields> 
                    </View>", PassValuesFuel[0], PassValuesFuel[1], PassValuesFuel[2]);
                    Result = InquirtyDetails("FuelTests", Caml, "FuelAmount");
                    break;
                case "Consumer":
                    string[] PassValuesCS = TradingLicenseNo.Split('|');
                    Caml = string.Format(@"<View>  
                                    <Query> 
                                       <Where><Eq><FieldRef Name='ID' /><Value Type='Text'>{1}</Value></Eq></Where> 
                                    </Query> 
                                     <ViewFields><FieldRef Name='Charges' /></ViewFields> 
                              </View>", PassValuesCS[0], PassValuesCS[1], PassValuesCS[2]);
                    Result = InquirtyDetails("ConsumerSafetyTest", Caml, "Charges");
                    break;
            }
            return Result;
        }

        public static string InquirtyDetails(string ListName, string Caml, string ReturnParamName)
        {
            string Result = string.Empty;
            Result = "-100";
            try
            {
                ClientContext clientContext = new ClientContext("http://intranet/sites/English/Sectors/ConsumerAndMarketServices/MarketSurveillance/QCCMeters");
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
                            Result = oListItem[ReturnParamName].ToString();
                            break;
                        }

                    }
                }




            }
            catch (Exception ex)
            {

                Result = "-100";
            }
            return Result;
        }


        public static string Payment(string ServiceName,string TradingLicenseNo,  string BankTransactionId)
        {
            string Result = string.Empty;
            string Caml = string.Empty;
            string Res = string.Empty;
            switch (ServiceName)
            {
                case "Scale":
                    string[] PassValues = TradingLicenseNo.Split('|');

                    Caml = string.Format(@"<View>    <Query> 
                                        <Where><Eq><FieldRef Name='ID' /><Value Type='Text'>{0}</Value></Eq></Where> </Query><ViewFields><FieldRef Name='BankTransactionID' /></ViewFields> 
                        </View>", PassValues[1]);
                    Res = InquirtyDetails("QCCScalesTests", Caml, "BankTransactionID");
                    if (Res == "" || Res == string.Empty)
                    {

                        Result = PaymentUtilzer(PassValues[1], "QCCScalesTests", "");
                    }
                    else
                    {
                        Result = "Reject | Already Paid";
                    }
                    break;
                case "Fuel":
                    string[] PassValuesFuel = TradingLicenseNo.Split('|');
                    Caml = string.Format(@"<View>    <Query> 
                                        <Where><Eq><FieldRef Name='ID' /><Value Type='Text'>{0}</Value></Eq></Where> </Query><ViewFields><FieldRef Name='BankTransactionID' /></ViewFields> 
                        </View>", PassValuesFuel[1]);
                    Res = InquirtyDetails("QCCScalesTests", Caml, "BankTransactionID");
                    if (Res == "" || Res == string.Empty)
                    {

                        Result = PaymentUtilzer(PassValuesFuel[1], "FuelTests", "");
                    }
                    else
                    {
                        Result = "Reject | Already Paid";
                    }
                    
                    break;
                case "Consumer":
                    string[] PassValuesCS = TradingLicenseNo.Split('|');
                    Caml = string.Format(@"<View>    <Query> 
                                        <Where><Eq><FieldRef Name='ID' /><Value Type='Text'>{0}</Value></Eq></Where> </Query><ViewFields><FieldRef Name='BankTransactionID' /></ViewFields> 
                        </View>", PassValuesCS[1]);
                    Res = InquirtyDetails("QCCScalesTests", Caml, "BankTransactionID");
                    if (Res == "" || Res == string.Empty)
                    {

                        Result = PaymentUtilzer(PassValuesCS[1], "ConsumerSafetyTest", "");
                    }
                    else
                    {
                        Result = "Reject | Already Paid";
                    }
                    break;
            }


            return Result;
        }
        public static string PaymentUtilzer(string ID, string ListName, string BankTransactionId)
        {
            string Res = string.Empty;
            try
            {
                var siteUrl = "http://intranet/sites/English/Sectors/ConsumerAndMarketServices/MarketSurveillance/QCCMeters";
                ClientContext clientContext = new ClientContext(siteUrl);
                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;
                List oList = clientContext.Web.Lists.GetByTitle(ListName);
                ListItem oListItem = oList.GetItemById(ID);
                oListItem["PaymentReportStatus"] = "PAID";
                oListItem["PaymentUpdateBy"] = "NBAD";
                oListItem["TestStage"] = "1600";
                oListItem["BankTransactionID"] = BankTransactionId;
                oListItem.Update();
                clientContext.ExecuteQuery();
                Res = "Accept";
            }
            catch (Exception)
            {
                Res = "Reject";
               
            }
            return Res;
        }



        public static List<ADQCCReconcile>[] ReconcileAll(string Keyword, string FromDate, string Todadate)
        {
            List<ADQCCReconcile> Data = new List<ADQCCReconcile>();
            List<ADQCCReconcile> [] Data1 = new List<ADQCCReconcile>[3];
           
            string Caml = string.Empty;
            try
            {
                        Caml = string.Format(@"<View>  
                                                    <Query> 
                                                       <Where><And><And><Geq><FieldRef Name='Created' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='Created' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And><Neq><FieldRef Name='TradingLiecense' /><Value Type='Text'>''</Value></Neq></And></Where> 
                                                    </Query> 
                                                     <ViewFields><FieldRef Name='TestStage' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='TradingLiecense' /><FieldRef Name='VerificationCharges' /><FieldRef Name='BankTransactionID' /><FieldRef Name='BankTransactionID' /></ViewFields> 
                                              </View>", FromDate, Todadate);
                        Data = InquirtyDetailsReconcile("QCCScalesTests", Caml, "Scale");
                        Data1[0] = Data;   
                   

                        Caml = string.Format(@"<View>   <Query>           <Where><And><Geq><FieldRef Name='Created' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='Created' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And></Where> </Query> <ViewFields><FieldRef Name='Title' /><FieldRef Name='Test_StationId' /><FieldRef Name='FuelAmount' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='Test_StationId' /><FieldRef Name='Created' /><FieldRef Name='TestStage' /><FieldRef Name='BankTransactionID' /></ViewFields>       </View>", FromDate, Todadate);
                        Data = InquirtyDetailsReconcile("FuelTests", Caml, "Fuel");
                        Data1[1] = Data;  
                   

                        Caml = string.Format(@"<View>   <Query>           <Where><And><Geq><FieldRef Name='Created' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='Created' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And></Where> </Query> <ViewFields><FieldRef Name='Title' /><FieldRef Name='TradeLiecense' /><FieldRef Name='Charges' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='TradeLiecense' /><FieldRef Name='Created' /><FieldRef Name='TestStage' /><FieldRef Name='BankTransactionID' /></ViewFields>       </View>", FromDate, Todadate);
                        Data = InquirtyDetailsReconcile("ConsumerSafetyTest", Caml, "Consumer");
                        Data1[2] = Data;



                       

            }
            catch (Exception ex)
            {
                // _ReconcileData.Error = ex.Message.ToString();

            }
            return Data1;
            //



        }
        public static List<ADQCCReconcile> Reconcile(string ServiceName, string FromDate, string Todadate)
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
                                                       <Where><And><And><Geq><FieldRef Name='Created' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='Created' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And><Neq><FieldRef Name='TradingLiecense' /><Value Type='Text'>''</Value></Neq></And></Where> 
                                                    </Query> 
                                                     <ViewFields><FieldRef Name='TestStage' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='TradingLiecense' /><FieldRef Name='VerificationCharges' /><FieldRef Name='BankTransactionID' /><FieldRef Name='BankTransactionID' /></ViewFields> 
                                              </View>", FromDate, Todadate);
                      Data=  InquirtyDetailsReconcile("QCCScalesTests", Caml,"Scale");
                        break;
                    case "Fuel":

                        Caml = string.Format(@"<View>   <Query>           <Where><And><Geq><FieldRef Name='Created' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='Created' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And></Where> </Query> <ViewFields><FieldRef Name='Title' /><FieldRef Name='Test_StationId' /><FieldRef Name='FuelAmount' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='Test_StationId' /><FieldRef Name='Created' /><FieldRef Name='TestStage' /><FieldRef Name='BankTransactionID' /></ViewFields>       </View>", FromDate, Todadate);
                        Data = InquirtyDetailsReconcile("FuelTests", Caml, "Fuel");
                        break;
                    case "Consumer":

                        Caml = string.Format(@"<View>   <Query>           <Where><And><Geq><FieldRef Name='Created' /><Value Type='DateTime'>{0}00:00:00Z</Value></Geq><Leq><FieldRef Name='Created' /><Value Type='DateTime'>{1}00:00:00Z</Value></Leq></And></Where> </Query> <ViewFields><FieldRef Name='Title' /><FieldRef Name='TradeLiecense' /><FieldRef Name='Charges' /><FieldRef Name='PaymentReportStatus' /><FieldRef Name='TradeLiecense' /><FieldRef Name='Created' /><FieldRef Name='TestStage' /><FieldRef Name='BankTransactionID' /></ViewFields>       </View>", FromDate, Todadate);
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
        public static List<ADQCCReconcile> InquirtyDetailsReconcile(string ListName, string Caml,string ServiceName)
        {
            List<ADQCCReconcile> FileList = new List<ADQCCReconcile>();
            string Result = string.Empty;
            Result = "-100";
            try
            {
                
                ClientContext clientContext = new ClientContext("http://intranet/sites/English/Sectors/ConsumerAndMarketServices/MarketSurveillance/QCCMeters");
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
                              string TradeLiecnese=string.Empty;
                             string Charges =string.Empty;
                            string ID=string.Empty;
                            switch (ServiceName)
                            {
                                case "Scale":
                                          
                                                   _File.Status =Convert.ToString(oListItem["TestStage"]);
                                                   TradeLiecnese = Convert.ToString(oListItem["TradingLiecense"]);
                                                   Charges = Convert.ToString(oListItem["VerificationCharges"]);
                                                   ID = Convert.ToString(oListItem["ID"]);
                                                   _File.TradeLiecnese = TradeLiecnese;
                                                   _File.Charges=Charges;
                                                   _File.TestIdentifier = TradeLiecnese + "|" + ID + "|" + Charges;
                                                   _File.TestDate = Convert.ToString(oListItem["Created"]);
                                                   _File.Status = Convert.ToString(oListItem["PaymentReportStatus"]);
                                                   _File.BankTransactionID = Convert.ToString(oListItem["BankTransactionID"]);
                                                   FileList.Add(_File);
                                    break;
                                case "Fuel":
                                    
                                    _File.Status = Convert.ToString(oListItem["TestStage"]);
                                    TradeLiecnese = Convert.ToString(oListItem["Test_StationId"]);
                                    Charges = Convert.ToString(oListItem["FuelAmount"]);
                                     ID = Convert.ToString(oListItem["ID"]);
                                    _File.TradeLiecnese = TradeLiecnese;
                                    _File.Charges = Charges;
                                    _File.TestIdentifier = TradeLiecnese + "|" + ID + "|" + Charges;
                                    _File.TestDate = Convert.ToString(oListItem["Created"]);
                                    _File.BankTransactionID = Convert.ToString(oListItem["BankTransactionID"]);
                                    FileList.Add(_File);
                                    break;

                                case "Consumer":
                                      _File.Status = Convert.ToString(oListItem["TestStage"]);
                                      TradeLiecnese = Convert.ToString(oListItem["TradeLiecense"]);
                                     Charges = Convert.ToString(oListItem["Charges"]);
                                     ID = Convert.ToString(oListItem["ID"]);
                                    _File.TradeLiecnese = TradeLiecnese;
                                    _File.Charges = Charges;
                                    _File.TestIdentifier = TradeLiecnese + "|" + ID + "|" + Charges;
                                    _File.TestDate = Convert.ToString(oListItem["Created"]);
                                    _File.BankTransactionID = Convert.ToString(oListItem["BankTransactionID"]);
                                    FileList.Add(_File);
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

    }
    public class ADQCCReconcile
    {
        
        public string Status { get; set; }
        public string Error { get; set; }
        public string Desc { get; set; }
        public string TradeLiecnese { get; set; }
        public string Result { get; set; }
        public string Stage { get; set; }
        public string Charges { get; set; }
        public string TestIdentifier { get; set; }
        public string TestDate { get; set; }
        public string BankTransactionID { get; set; }
        public string ServiceType { get; set; }
        public string BarCodeString { get; set; }

    }
}
