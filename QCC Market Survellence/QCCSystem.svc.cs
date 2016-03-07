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
namespace QCC_Market_Survellence
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "QCCSystem" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select QCCSystem.svc or QCCSystem.svc.cs at the Solution Explorer and start debugging.
    public class QCCSystem : IQCCSystem
    {
        public void DoWork()
        {
        }
        public string GetData(int value)
        {
            return "sdf";
        }
        public string GetInformation(string type, string fromdt, string todt)
        {
            string Result = type;
            switch (type)
            {
                case "Scales":
                    Result = Scalesdata(fromdt, todt);
                    break;
                case "Fuels":
                   Result = GetFuelData(fromdt, todt);
                    break;
                case "CS":
                    Result = ConsumerSafetyData(fromdt, todt);
                    break;

            }
            return Result;
        
        }
        public string GetFuelData(string fromdt, string todt)
        {
            string Result = "";
            try
            {
                ClientContext clientContext = new ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets");
                SP.List oList = clientContext.Web.Lists.GetByTitle("Budget");
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='Sector'/><Value Type='Text'>" + fromdt + "</Value></Eq></Where></Query><RowLimit>900</RowLimit></View>";
                // camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='Sector'/>" + "<Value Type='Text'>" + Fin + "</Value> </Eq></Where></Query><RowLimit>100</RowLimit></View>";
                ListItemCollection collListItem = oList.GetItems(camlQuery);
                clientContext.Load(collListItem);
                clientContext.ExecuteQuery();
                DataTable table = new DataTable();
                table.Columns.Add("Category", typeof(string));
                table.Columns.Add("AccountNo", typeof(string));
                table.Columns.Add("Descreption", typeof(string));
                table.Columns.Add("MonthlySalary", typeof(string));
                table.Columns.Add("Months", typeof(string));
                table.Columns.Add("Cost", typeof(string));
                table.Columns.Add("CostCenter", typeof(string));
                table.Columns.Add("Sector", typeof(string));
                table.Columns.Add("Number", typeof(string));
                table.Columns.Add("Total", typeof(string));
                table.Columns.Add("Proposed", typeof(string));
                table.Columns.Add("CountNo", typeof(string));
                table.Columns.Add("Notes", typeof(string));
                table.Columns.Add("ID", typeof(string));


                foreach (ListItem oListItem in collListItem)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = oListItem["Category"].ToString();
                    dr[1] = oListItem["AccountNo"].ToString();
                    dr[2] = oListItem["Descreption"].ToString();
                    dr[3] = oListItem["MonthlySalary"].ToString();
                    dr[4] = oListItem["Months"].ToString();
                    dr[5] = oListItem["Cost"].ToString();
                    dr[6] = oListItem["CostCenter"].ToString();
                    dr[7] = oListItem["Sector"].ToString();
                    dr[8] = oListItem["Number"].ToString();
                    dr[9] = oListItem["Total"].ToString();
                    dr[10] = oListItem["Proposed"].ToString();
                    dr[11] = oListItem["CountNo"].ToString();
                    dr[12] = oListItem["Notes"].ToString();
                    dr[13] = oListItem["ID"].ToString();
                    table.Rows.Add(dr);
                }

                DataTable uniqueCols = table.DefaultView.ToTable(true, "Category");
                for (int x = 0; x < uniqueCols.Rows.Count; x++)
                {
                    string _FindMode = uniqueCols.Rows[x]["Category"].ToString();
                    Result += "<h1>" + _FindMode + "</h1>";
                    Result += "<table border='1' style='width:100%;'> <tr>   <td class='newStyle1'> Descreption</td><td class='newStyle1'>No	Cost </td><td class='newStyle1'>Months </td><td class='newStyle1'>Total </td><td class='newStyle1'> Notes</td><td class='newStyle1'> Cost Center</td><td class='newStyle1'>Attachment</td>    </tr>";

                    DataRow[] result = uniqueCols.Select(" Category ='" + _FindMode + "'");
                    DataTable tablex = table.Clone();
                    DataRow[] foundRows2;
                    foundRows2 = table.Select("Category='" + _FindMode + "'");
                    foreach (DataRow temp in foundRows2)
                    {
                        tablex.ImportRow(temp);
                    }
                    double count = 0;
                    double Temp = 0;
                    for (int z = 0; z < tablex.Rows.Count; z++)
                    {

                        if (tablex.Rows[z]["Cost"].ToString() != null && tablex.Rows[z]["Cost"].ToString() != string.Empty)
                        {
                            count = double.Parse(tablex.Rows[z]["Cost"].ToString());
                            Temp += count;
                        }
                        Result += "<tr> <td>" + tablex.Rows[z]["Descreption"].ToString() + "</td> <td>" + tablex.Rows[z]["MonthlySalary"].ToString() + "</td>  <td>" + tablex.Rows[z]["Months"].ToString() + "</td> <td>" + tablex.Rows[z]["Notes"].ToString() + "</td>              <td>" + tablex.Rows[z]["CostCenter"].ToString() + "</td>       <td><a onclick='LinktoDocument(" + tablex.Rows[z]["ID"].ToString() + ")' > Document</a> </td> <td><a id=" + tablex.Rows[z]["ID"].ToString() + " onclick='DeleteThisItem(" + tablex.Rows[z]["ID"].ToString() + ")' >Delete </a></td>      </tr>";
                    }
                    Result += "<tr><td colspan='6' style='background-color: #CCCCCC'> <h2>Total </h2>  </td > <td  style='background-color: #CCCCCC'> " + Temp + " </td></tr> ";
                    Result += "</table> <br> ";

                }


            }
            catch (Exception ex)
            {

                Result = "Finance Budget System";
            }
            
            return Result;
        }
        public string Scalesdata(string fromdt, string todt)
        {


            //string Result="";// = fromdt + todt;
            string Result = "";//; fromdt + todt;
            try
            {
                //Result += "1";
                ClientContext clientContext = new ClientContext("http://intranet/sites/English/Sectors/ConsumerAndMarketServices/MarketSurveillance/QCCMeters/");
                SP.List oList = clientContext.Web.Lists.GetByTitle("QCCScalesTests");
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View><Query><Where><And><And><Geq><FieldRef Name='Created' /><Value IncludeTimeValue='TRUE'  Type='DateTime'>" + fromdt + " T 00:00:00Z</Value></Geq><Leq><FieldRef Name='Created' /><Value IncludeTimeValue='TRUE' Type='DateTime'>" + todt + " T 00:00:00Z</Value>" + " </Leq></And><Eq> <FieldRef Name='ArModuleSync' /> <Value Type='Text'>NO</Value>  </Eq> </And> </Where></Query><RowLimit>100</RowLimit></View>";
                
                ListItemCollection collListItem = oList.GetItems(camlQuery);
                clientContext.Load(collListItem);
                clientContext.ExecuteQuery();
                //Result += "2";
                DataTable table = new DataTable();
               // Result += "3";
                table.Columns.Add("TradingLiecense", typeof(string));
                table.Columns.Add("VerificationConductedBy", typeof(string));
                table.Columns.Add("ScaleCalss", typeof(string));
                table.Columns.Add("FinalTestResults", typeof(string));
                table.Columns.Add("VerificationCharges", typeof(string));
                table.Columns.Add("ID", typeof(string));
                table.Columns.Add("Created", typeof(string));
                table.Columns.Add("TestStage", typeof(string));
                
               // Result += "4";

                foreach (ListItem oListItem in collListItem)
                {
                    //Result += "5";
                    DataRow dr = table.NewRow();
                    dr[0] = oListItem["TradingLiecense"].ToString();
                    //Result += "6";
                    dr[1] = oListItem["VerificationConductedBy"].ToString();
                   // Result += "7";
                    dr[2] = oListItem["ScaleCalss"].ToString();
                    //Result += "8";
                    dr[3] = oListItem["FinalTestResults"].ToString();
                    //Result += "9";
                    dr[4] = oListItem["FinalTestResults"].ToString();
                    //Result += "10";
                    dr[5] = oListItem["ID"].ToString();
                    dr[6] = oListItem["Created"].ToString();
                    dr[7] = oListItem["TestStage"].ToString();
                    //Result += "11";
                    table.Rows.Add(dr);
                }

                DataTable table2 = new DataTable();
                table2 = table.Clone();

                //1 Rows
                DataRow[] foundRows;
                foundRows = table.Select("TestStage <>500 AND TestStage <>200  AND TestStage <>300 ");
                //foundRows = table.Select("pos = '" + Noc2 + "' and Position='" + Position + "'");
                foreach (DataRow temp in foundRows)
                {
                    table2.ImportRow(temp);
                }


                //Result += table.Rows.Count.ToString();
                DataTable uniqueCols = table2.DefaultView.ToTable(true, "ID");
                Result += "<table border='1' style='width:100%;'> <tr>   <td class='newStyle1'> Trading Liecense</td><td class='newStyle1'>Inspector </td><td class='newStyle1'>Class </td><td class='newStyle1'>Result </td><td class='newStyle1'> Fees</td><td class='newStyle1'>Test Date </td><td>Details</td>    </tr>";
                for (int z = 0; z < uniqueCols.Rows.Count; z++)
                {
                    Result += "<tr> <td>" + table.Rows[z]["TradingLiecense"].ToString() + "</td> <td>" + table.Rows[z]["VerificationConductedBy"].ToString() + "</td>  <td>" + table.Rows[z]["ScaleCalss"].ToString() + "</td> <td>" + table.Rows[z]["FinalTestResults"].ToString() + "</td>              <td>" + table.Rows[z]["VerificationCharges"].ToString() + " AED </td>        <td>" + table.Rows[z]["Created"].ToString() + " </td><td><a id=" + table.Rows[z]["ID"].ToString() + " onclick='dimOn(" + table.Rows[z]["ID"].ToString() + ")' > Detail </a></td>      </tr>";

                }
                Result += "</table> <br> ";



            }
            catch (Exception ex)
            {

                Result += ex.Message.ToString();
                // string Result = string.Empty;
            }
            return Result;
        }
        public string ConsumerSafetyData(string fromdt, string todt)
        {
            string Result = string.Empty;
            return Result;
        }
        public string GetBudgetReports(string type)
        {
            double OverAllTotalProposed = 0.0;
            double OverAllTotalApproved = 0.0;
            string X = string.Empty;
            DataTable _Budget = GetBudgetMainSector();
            DataTable AprovedBudget = GetApprovedBudget();

            DataView view = new DataView(_Budget);
            DataTable distinctValues = view.ToTable(true, "MainSector");


            string Result1 = " <div id='selecttype' border='0' style='width:100%;'> <table border='0' style='width:100%;'>   <tr>   <td style='font-size:13px;'>Select AED / Percentage  [%] <select id='selectypex' style='width:120px;Height:40px' onchange='selecttype()'><option>Percentage</option> <option>Number</option>    </select>   </td>   <td>Remaining  <div id='remainingamount'></div><span class='Type'>%</span></td>  </tr></table>  <div> <table border='1' style='width:100%;'> <tr>   <td  style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'> Sector Name</td><td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Total  Budget Proposed </td><td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Total  Budget Proposed (%) </td><td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Total  Budget Approved</td><td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Total  Budget Approved (%)</td><td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Allocated </td> </tr>";
            DataTable tablex = new DataTable();
            DataTable table = new DataTable();
            tablex.Columns.Add("ID", typeof(string));
            tablex.Columns.Add("MainSector", typeof(string));
            tablex.Columns.Add("Total", typeof(string));
            tablex.Columns.Add("Project", typeof(string));
            tablex.Columns.Add("Proposed", typeof(string));
            double SumofApprovedBudgetX = 0.0;
            double SumofproposedBudgetX = 0.0;
            string AllSectors = string.Empty;

            double SumofApprovedBudget = 0.0;
            double SumofproposedBudget = 0.0;
            for (int j = 0; j < _Budget.Rows.Count; j++)
            {
                string proposed = _Budget.Rows[j]["Proposed"].ToString();
                string approved = _Budget.Rows[j]["Total"].ToString();
                if (proposed != string.Empty && approved != string.Empty)
                {
                    double temp1 = double.Parse(proposed);
                    double temp2 = double.Parse(approved);

                     SumofproposedBudgetX += temp1;
                     SumofApprovedBudgetX += temp2;
                }
            }

           
            for (int x = 0; x < distinctValues.Rows.Count; x++)
            {

                DataRow[] foundRows2;
                foundRows2 = _Budget.Select("MainSector='" + distinctValues.Rows[x]["MainSector"].ToString() + "'");
                tablex.Rows.Clear();
                foreach (DataRow temp in foundRows2)
                {
                    tablex.ImportRow(temp);
                }
                SumofApprovedBudget = 0.0;
                SumofproposedBudget = 0.0;
                for (int y = 0; y < tablex.Rows.Count; y++)
                {
                    string TempSume = tablex.Rows[y]["Total"].ToString();
                    if (TempSume != string.Empty)
                    {

                        double temp2 = double.Parse(TempSume);
                        SumofApprovedBudget += temp2;
                       // SumofApprovedBudgetX += temp2;
                       

                    }
                    string TempSume1 = tablex.Rows[y]["Proposed"].ToString();
                    if (TempSume1 != string.Empty)
                    {
                        double temp21 = double.Parse(TempSume1);
                        SumofproposedBudget += temp21;
                    }
                }
                double Perceproposed = SumofproposedBudget / SumofproposedBudgetX;
                Perceproposed = Perceproposed * 100;
                double Perceapproved = SumofApprovedBudget / SumofApprovedBudgetX;
                Perceapproved = Perceapproved * 100;

                AllSectors += distinctValues.Rows[x]["MainSector"].ToString().Replace("&", "-").ToString() + "#";
                Result1 += "<tr><td style='font-size:13px'>" + distinctValues.Rows[x]["MainSector"].ToString().Replace("&", "-").ToString() + " </td><td style='font-size:13px'>" + SumofproposedBudget.ToString("#,##0.##") + " </td><td style='font-size:13px'>" + Perceproposed.ToString("#.##") + "%</td><td style='font-size:13px'>" + SumofApprovedBudget.ToString("#,##0.##") + " </td><td style='font-size:13px'>" + Perceapproved.ToString("#.##") + "%</td><td> <input type='text' id='" + distinctValues.Rows[x]["MainSector"].ToString().Replace(" ", "").ToString().Replace("&", "-").ToString() + "'  style='width:240px' /><span class='Type'>%</span></td></tr>";
            }
            Result1 += "<tr><td style='font-size:20px'>Total</td><td style='font-size:13px'> " + SumofproposedBudgetX.ToString("#,##0.##") + "</td><td style='font-size:13px'></td><td style='font-size:13px'><div id='approvedbudget'> " + SumofApprovedBudgetX.ToString("#,##0.##") + "</div><div id='approvedbudget1' style='display:none'> " + SumofApprovedBudgetX.ToString().Replace(",","") + "</div></td><td style='font-size:13px'></td><td style='font-size:20px'></td></tr>";
            AllSectors = AllSectors.ToString().Replace(" ", "").ToString();
            Result1 += "</table><table border='0' style='width:100%;'><tr><td align='right'><input type='button' value='Allocate Quota' onclick='SubmitBudget()' /></td></tr></table> <br><br><div id='messages'></div><div id='hiddensectors' style='display:none'>" + AllSectors + "</div> ";

            X = Result1;
            string Result = string.Empty;
            Result = "<table border='1' style='width:100%;'> <tr>   <td style='font-size:30px;color: #FFFFFF; background-color: #3399FF;'> Project Name</td><td style='font-size:30px;color: #FFFFFF; background-color: #3399FF;'>Proposed Budget</td> <td style='font-size:30px;color: #FFFFFF; background-color: #3399FF;'>Approved Budget</td><td style='font-size:30px;color: #FFFFFF; background-color: #3399FF;'>Deviation</td></tr>";
            try
            {
                double count = 0;
                double Temp = 0;

                for (int i = 0; i < _Budget.Rows.Count; i++)
                {


                    double _BeginTotal = double.Parse(_Budget.Rows[i]["Total"].ToString());
                    double _EndTotal = double.Parse(AprovedBudget.Rows[i]["Total"].ToString());
                    double Spent = _BeginTotal - _EndTotal;

                    Result += "<tr> <td style='font-size:20px'>" + _Budget.Rows[i]["Project"].ToString() + "</td> <td style='font-size:20px'>" + _BeginTotal.ToString("#,##0.##") + " AED </td><td style='font-size:20px'>" + _EndTotal.ToString("#,##0.##") + " AED </td>            <td style='font-size:20px'>" + Spent.ToString() + "</td></td>      </tr>";
                }
                Result += "</table>";

            }

            catch (Exception ex)
            {

                Result = "Finance Budget System";
            }
            X += "|" + Result;
            return X;
        }
        public DataTable GetBudget()
        {
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
            SP.List oList = clientContext.Web.Lists.GetByTitle("Budget");
            SP.CamlQuery camlQuery = new SP.CamlQuery();
            camlQuery.ViewXml = "<View><Query><Where><Gt><FieldRef Name='ID'/>" + "<Value Type='Counter'>1</Value></Gt></Where></Query></View>";
            SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Sector", typeof(string));
            table.Columns.Add("Total", typeof(string));
            table.Columns.Add("Project", typeof(string));
            table.Columns.Add("Proposed", typeof(string));
            foreach (SP.ListItem oListItem in collListItem)
            {
                DataRow dr = table.NewRow();
                dr[0] = oListItem["ID"];
                dr[1] = oListItem["Sector"];
                dr[2] = oListItem["Total"];
                dr[3] = oListItem["Project"];
                dr[4] = oListItem["Proposed"];
                table.Rows.Add(dr);
            }
            return table;

        }
        public DataTable GetApprovedBudget()
        {
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
            SP.List oList = clientContext.Web.Lists.GetByTitle("Budget");
            SP.CamlQuery camlQuery = new SP.CamlQuery();
            //camlQuery.ViewXml = "<View><Query><Where><And><Eq><FieldRef Name='Sector'/>" + "<Value Type='Text'>" + a + "</Value> </Eq><Eq><FieldRef Name='CAPEX'/>" + "<Value Type='Text'>" + b + "</Value></Eq></And></Where></Query><RowLimit>100</RowLimit></View>";
            camlQuery.ViewXml = "<View><Query><Where><Gt><FieldRef Name='ID'/>" + "<Value Type='Counter'>1</Value></Gt></Where></Query></View>";
            SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Sector", typeof(string));
            table.Columns.Add("Total", typeof(string));
            table.Columns.Add("Project", typeof(string));
            table.Columns.Add("Proposed", typeof(string));
            foreach (SP.ListItem oListItem in collListItem)
            {
                DataRow dr = table.NewRow();
                dr[0] = oListItem["ID"];
                dr[1] = oListItem["Sector"];
                dr[2] = oListItem["Total"];
                dr[3] = oListItem["Project"];
                dr[4] = oListItem["Proposed"];
                table.Rows.Add(dr);
            }
            return table;

        }
        public DataTable GetApprovedBudgetbysector()
        {
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
            SP.List oList = clientContext.Web.Lists.GetByTitle("Budget");
            SP.CamlQuery camlQuery = new SP.CamlQuery();
            //camlQuery.ViewXml = "<View><Query><Where><And><Eq><FieldRef Name='Sector'/>" + "<Value Type='Text'>" + a + "</Value> </Eq><Eq><FieldRef Name='CAPEX'/>" + "<Value Type='Text'>" + b + "</Value></Eq></And></Where></Query><RowLimit>100</RowLimit></View>";
            camlQuery.ViewXml = "<View><Query><Where><Gt><FieldRef Name='ID'/>" + "<Value Type='Counter'>1</Value></Gt></Where></Query></View>";
            SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Sector", typeof(string));
            table.Columns.Add("Total", typeof(string));
            table.Columns.Add("Project", typeof(string));
            foreach (SP.ListItem oListItem in collListItem)
            {
                DataRow dr = table.NewRow();
                dr[0] = oListItem["ID"];
                dr[1] = oListItem["Sector"];
                dr[2] = oListItem["Total"];
                dr[3] = oListItem["Project"];
                table.Rows.Add(dr);
            }
            return table;

        }
        public string UpdateIteminformatoin(string Officer, string officerEmail, string ProjectType, string PurchasingType, string Delivrables, string QccStarategyMap, string ProcStartDate, string ID, string strtcatagories, string strtsubcat, string Divisions)
        {
            string Res = string.Empty;

            var siteUrl = "http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets";
            ClientContext clientContext = new ClientContext(siteUrl);
            NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
            clientContext.Credentials = credentials;
            List oList = clientContext.Web.Lists.GetByTitle("Budget");
            ListItem oListItem = oList.GetItemById(ID);
            oListItem["Officer"] = Officer;
            oListItem["OfficerEmail"] = officerEmail;
            oListItem["ProjectType"] = ProjectType;
            oListItem["PurchasingType"] = PurchasingType;
            oListItem["Delivrables"] = Delivrables;
            oListItem["QccStarategyMap"] = QccStarategyMap;
            oListItem["ProcStartDate"] = ProcStartDate;
            oListItem["QCCStrategyCatagories"] = strtcatagories;
            oListItem["QCCStrategySubCatagories"] = strtsubcat;
            oListItem["Stage"] = "3";
            oListItem["MainDivision"] = Divisions;
            oListItem.Update();
            clientContext.ExecuteQuery();
            Res = "SuccessFully Added the Procurement Plan!!!!";

            return Res;
        }
        public  DataTable GetApprovedBudgetbysector2()
        {
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
            SP.List oList = clientContext.Web.Lists.GetByTitle("Budget");
            SP.CamlQuery camlQuery = new SP.CamlQuery();
            //camlQuery.ViewXml = "<View><Query><Where><And><Eq><FieldRef Name='Sector'/>" + "<Value Type='Text'>" + a + "</Value> </Eq><Eq><FieldRef Name='CAPEX'/>" + "<Value Type='Text'>" + b + "</Value></Eq></And></Where></Query><RowLimit>100</RowLimit></View>";
            camlQuery.ViewXml = "<View><Query><Where><Gt><FieldRef Name='ID'/>" + "<Value Type='Counter'>1</Value></Gt></Where></Query></View>";
            SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Sector", typeof(string));
            table.Columns.Add("Total", typeof(string));
            table.Columns.Add("Project", typeof(string));
            table.Columns.Add("Proposed", typeof(string));
            table.Columns.Add("Division", typeof(string));
            foreach (SP.ListItem oListItem in collListItem)
            {
                DataRow dr = table.NewRow();
                dr[0] = oListItem["ID"];
                dr[1] = oListItem["Sector"];
                dr[2] = oListItem["Total"];
                dr[3] = oListItem["Project"];
                dr[4] = oListItem["Proposed"];
                dr[5] = oListItem["Division"];

                table.Rows.Add(dr);
            }
            return table;

        }
        public  DataTable GetDivisionofSector(string Sector)
        {
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
            SP.List oList = clientContext.Web.Lists.GetByTitle("QCCChart");
            SP.CamlQuery camlQuery = new SP.CamlQuery();
            //camlQuery.ViewXml = "<View><Query><Where><And><Eq><FieldRef Name='Sector'/>" + "<Value Type='Text'>" + a + "</Value> </Eq><Eq><FieldRef Name='CAPEX'/>" + "<Value Type='Text'>" + b + "</Value></Eq></And></Where></Query><RowLimit>100</RowLimit></View>";
            camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='Sector'/>" + "<Value Type='Text'>" + Sector + "</Value></Eq></Where></Query></View>";
            SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Sector", typeof(string));
            table.Columns.Add("Division", typeof(string));
            foreach (SP.ListItem oListItem in collListItem)
            {
                DataRow dr = table.NewRow();
                dr[0] = oListItem["ID"].ToString();
                dr[1] = oListItem["Sector"].ToString();
                dr[2] = oListItem["Division"].ToString();

                table.Rows.Add(dr);
            }
            return table;

        }
        public string GetExecutiveDirectory(string Sector)
        {
            string X = string.Empty;
            DataTable AprovedBudget = GetApprovedBudgetbysector2();
            DataTable Divisionc = GetDivisionofSector(Sector);
            DataView view = new DataView(Divisionc);
            DataTable distinctValues = view.ToTable(true, "Division");
            DataTable tablex = new DataTable();
            DataTable table = new DataTable();
            string Temp = Sector.ToString().Replace(" ", "").ToString().TrimEnd().ToString();
            string AllocatedBugettoThisSector = GetAllocatedBudgetBySectorOrDivision(Temp, "Sector");
            //string AllocatedBugettoThisSector = GetAllocatedBudgetBySectorOrDivision("CorporateSupportServices", "Sector");
            //AllocatedBugettoThisSector += Sector;
            tablex.Columns.Add("ID", typeof(string));
            tablex.Columns.Add("Sector", typeof(string));
            tablex.Columns.Add("Total", typeof(string));
            tablex.Columns.Add("Project", typeof(string));
            tablex.Columns.Add("Proposed", typeof(string));
            tablex.Columns.Add("Division", typeof(string));


            DataRow[] foundRows2;
            foundRows2 = AprovedBudget.Select("Sector='" + Sector + "'");
            double SumofApprovedBudget = 0.0;
            double SumofApprovedBudgetProposed = 0.0;
            foreach (DataRow temp in foundRows2)
            {
                tablex.ImportRow(temp);
            }


            for (int y = 0; y < tablex.Rows.Count; y++)
            {
                //Approved
                string TempSume = tablex.Rows[y]["Total"].ToString();
                double temp2 = double.Parse(TempSume);
                SumofApprovedBudget += temp2;

                //Prpposed
                string TempSumeProposed = tablex.Rows[y]["Proposed"].ToString();
                double temp2Proposed = double.Parse(TempSumeProposed);
                SumofApprovedBudgetProposed += temp2Proposed;


            }
            double Devi = SumofApprovedBudget - SumofApprovedBudgetProposed;
            double Perce = Devi / SumofApprovedBudget;
            Perce = Perce * 100;

            //string AnotherTable = "<div id='selecttype'><table>   <tr>      <td> <input type='radio'  name='percentage'>Percentage<input type='radio' name='percentage'>Number</td>     </tr></table>  <div> <table border='1'><tr><td>Division</td> <td>Total Amount</td> <td>Allocate Amount</td> </tr>";
            string AnotherTable = "<div id='selecttype' border='0' style='width:100%;'> <table border='0' style='width:100%;'>   <tr>   <td style='font-size:20px;'>Select AED / Percentage  [%] <select id='selectypex' style='width:120px' onchange='selecttype()'><option>Percentage</option> <option>Number</option>    </select></td>     </tr></table>  <div> <table id='selecttype' border='1' style='width:100%;'><tr><td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Division</td><td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Proposed (Number)</td><td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Proposed (%) </td><td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Approved (Number)</td><td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Apporved (%)</td> <td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Allocate Amount</td>";
            
            double _DivisoinsAmountproposed = 0.0;
            double _DivisoinsAmount = 0.0;
            string DiviName = string.Empty;
            string DiviName1 = string.Empty;
            for (int i = 0; i < distinctValues.Rows.Count; i++)
            {
                if (distinctValues.Rows[i]["Division"].ToString().ToLower().ToString() != "none")
                {
                    _DivisoinsAmount = 0.0;
                    _DivisoinsAmountproposed = 0.0;
                    for (int z = 0; z < tablex.Rows.Count; z++)
                    {
                        if (tablex.Rows[z]["Division"].ToString() == distinctValues.Rows[i]["Division"].ToString())
                        {
                            _DivisoinsAmount += double.Parse(tablex.Rows[z]["Total"].ToString());
                            _DivisoinsAmountproposed += double.Parse(tablex.Rows[z]["Proposed"].ToString());
                        }
                    }
                    DiviName = distinctValues.Rows[i]["Division"].ToString().Trim().ToString().Replace(" ", "").ToString();
                    DiviName1 += distinctValues.Rows[i]["Division"].ToString().Trim().ToString().Replace(" ", "").ToString() + "|";
                    double ProposedPercentageDivision = 0.0;
                    double approvedPercentageDivision = 0.0;
                    double divpercentage = _DivisoinsAmountproposed/  SumofApprovedBudgetProposed;
                    double temppercentage = divpercentage *100;
                   // temppercentage = temppercentage *100;
                    double divpercentageappr = _DivisoinsAmount /SumofApprovedBudget - _DivisoinsAmount;
                    double temppercentage3 = divpercentage * 100;
                    //temppercentage3 = temppercentage3 * 100;
                    if (temppercentage == 0)
                    {
                        temppercentage = 0.0;
                    }
                    AnotherTable += "<tr><td style='font-size:20px;'>" + distinctValues.Rows[i]["Division"].ToString() + "</td><td style='font-size:13px;'> " + _DivisoinsAmountproposed.ToString("#,##0.##") + " AED </td> <td style='font-size:13px;'> " + temppercentage.ToString("#.##") + " % </td><td style='font-size:13px;'> " + _DivisoinsAmount.ToString("#,##0.##") + " AED </td><td style='font-size:13px;'> " + temppercentage3.ToString("#.##") + "%</td><td> <input type='text' id='" + DiviName + "' /> <span class='Type'>%</span></td> </tr>";
                }
            }
            AnotherTable += "</table> <input type='hidden' value='" + DiviName1 + "' id='hiddendivsion' /> ";
            string Result1 = "<table border='1' style='width:90%;'> <tr>                   <td   style='font-size:30px;color: #FFFFFF; background-color: #3399FF;'> Sector Name</td><td style='font-size:20px;'><div id='sectorname'> " + Sector + "</div> </td>        </tr>         <tr>   <td  style='font-size:30px;color: #FFFFFF; background-color: #3399FF;'> Total  Propose Budget </td> <td style='font-size:20px;'>" + SumofApprovedBudgetProposed.ToString("#,##0.##") + " </td>                </tr>                <tr><td  style='font-size:30px;color: #FFFFFF; background-color: #3399FF;'> Total  Approved Budget </td><td ><div id='approvedbudget' style='font-size:20px;'> " + SumofApprovedBudget.ToString("#,##0.##") + " </div><div id='approvedbudgethidden' style='font-size:20px;display:none'> " + SumofApprovedBudget.ToString() + " </div></td>   </tr>                    <tr><td  style='font-size:30px;color: #FFFFFF; background-color: #3399FF;'> Total  Allocated Budget </td><td  style='font-size:30px;'>" + AllocatedBugettoThisSector + " AED</td>   </tr>                    </table>";
            X = Result1 + "<br><br>" + AnotherTable + "<table border='0' style='width:100%;'><tr><td style='text-align:right'><input type='button' value='Allocate Quota' onclick='SubmitBudget()' class='buttonsx' /></td></tr></table> <br><br><div id='messages'></div>";
            return X;
        }
        public string SetDivisionQuota(string Sector, string DivisionData,string Type,string Total)
        {
           // throw new Exception(DivisionData);
            string REsult = "Successfully Allocated Amount to Division";

            try
            {
                string[] DivisionParser = DivisionData.Split('|');
                for (int i = 0; i < DivisionParser.Length-1; i++)
                {
                    string[] DataParser = DivisionParser[i].ToString().Split(':');
                    string MainDivision = DataParser[0];
                    string Amount = DataParser[1];
                    double AmountTemp = 0.0;
                    double TotalAmount = double.Parse(Total);
                    if (Type == "Percentage" && Amount !=null)
                    {
                        double temp = double.Parse(Amount);
                        AmountTemp = TotalAmount * temp / 100;
                    }
                    else
                    {
                        AmountTemp = double.Parse(Amount);
                    }
                    if (CheckDivisionFoundIntheList(Sector, MainDivision, AmountTemp.ToString(), Type) == "No")
                    { }
                    else
                    {
                        SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
                        SP.List oList = clientContext.Web.Lists.GetByTitle("DivisionAllocatedBudget");
                        NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                        clientContext.Credentials = credentials;
                        ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                        ListItem oListItem = oList.AddItem(itemCreateInfo);
                        oListItem["Sector"] = Sector;
                        oListItem["Division"] = MainDivision;
                        oListItem["AmountAllocated"] = AmountTemp.ToString();
                        oListItem["Types"] = Type;
                        oListItem.Update();
                        clientContext.ExecuteQuery();
                    }
                }

               
            }
            catch (Exception ex)
            {

               // throw new Exception("Error Contact Administrator");
                REsult = ex.Message.ToString() + DivisionData.ToString();
            }
            //return "Successfully Allocated Amount to Division";
            return REsult;
        }
        public string GetAllocatedBudgetBySectorOrDivision(string SectorOrDivision, string Key)
        {
            string Result = string.Empty;
            try
            {
                DataTable _SectorAllocatedBudgetTable = new System.Data.DataTable();
                string ListName = "";
                string ColumnName = "";
                string Caml = "";
                if (Key == "Sector")
                {
                    ListName = "SectorsAllocatedBudget";
                    ColumnName = "AllocatedBudget";
                    Caml = "<View><Query><Where><Eq><FieldRef Name='SectorName'/>" + "<Value Type='Text'>" + SectorOrDivision + "</Value></Eq></Where></Query></View>";
                }
                else
                {
                    ListName = "DivisionAllocatedBudget";
                    ColumnName = "AmountAllocated";
                    Caml = "<View><Query><Where><Eq><FieldRef Name='Sector'/>" + "<Value Type='Text'>" + SectorOrDivision + "</Value></Eq></Where></Query></View>"; ;
                }
                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
                SP.List oList = clientContext.Web.Lists.GetByTitle(ListName);
                SP.CamlQuery camlQuery = new SP.CamlQuery();
                camlQuery.ViewXml = Caml;
                SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
                clientContext.Load(collListItem);
                clientContext.ExecuteQuery();
                DataTable table = new DataTable();
                table.Columns.Add(ColumnName, typeof(string));
                foreach (SP.ListItem oListItem in collListItem)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = oListItem[ColumnName];
                    table.Rows.Add(dr);
                }

                for (int x = 0; x < table.Rows.Count; x++)
                {
                    Result += table.Rows[x][ColumnName].ToString();
                }


            }
            catch (Exception ex)
            {

                Result = ex.Message.ToString();
            }
            return Result;
        }
        public string GetDivisionLevelProjects(string Division)
        {
            string allocatedBudget = string.Empty;
            Division = Division.Trim().ToString().TrimEnd().ToString();
            string Result = string.Empty;
            try
            {
                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
                SP.List oList = clientContext.Web.Lists.GetByTitle("Budget");

                SP.CamlQuery camlQuery = new SP.CamlQuery();
                camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='Division'/>" +
                 "<Value Type='Text'>" + Division + "</Value></Eq></Where></Query></View>";
                SP.ListItemCollection collListItem = oList.GetItems(camlQuery);

                clientContext.Load(collListItem);

                clientContext.ExecuteQuery();

                string strDiv = Division.Replace(" ", "");
                #region Get Allocated Budget 
                SP.ClientContext clientContextDivision = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
                SP.List oListDivision = clientContextDivision.Web.Lists.GetByTitle("DivisionAllocatedBudget");

                SP.CamlQuery camlQueryDivision = new SP.CamlQuery();
                camlQueryDivision.ViewXml = "<View><Query><Where><Eq><FieldRef Name='Division'/>" +
                  "<Value Type='Text'>" + strDiv + "</Value></Eq></Where></Query></View>";
                SP.ListItemCollection collListItemDivision = oListDivision.GetItems(camlQueryDivision);

                clientContextDivision.Load(collListItemDivision);

                clientContextDivision.ExecuteQuery();
                #endregion
                allocatedBudget = collListItemDivision[0]["AmountAllocated"].ToString();


                DataTable tableMain = new DataTable();

                tableMain.Columns.Add("Project", typeof(string));
                tableMain.Columns.Add("Cost", typeof(string));
                tableMain.Columns.Add("Proposed", typeof(string));
                tableMain.Columns.Add("Total", typeof(string));
                tableMain.Columns.Add("Quantity", typeof(string));
                tableMain.Columns.Add("Descreption", typeof(string));
                tableMain.Columns.Add("ID", typeof(string));
                tableMain.Columns.Add("Section", typeof(string));

                foreach (SP.ListItem oListItem in collListItem)
                {
                    DataRow dr = tableMain.NewRow();
                    dr[0] = oListItem["Project"];
                    dr[1] = oListItem["Cost"];
                    dr[2] = oListItem["Proposed"];
                    dr[3] = oListItem["Total"];
                    dr[4] = oListItem["Quantity"];
                    dr[5] = oListItem["Descreption"];
                    dr[6] = oListItem["ID"];
                    dr[7] = oListItem["Section"];
                    tableMain.Rows.Add(dr);
                }

                DataRow[] foundRows2;
                foundRows2 = tableMain.Select("Section = 'none'");
                DataTable table = tableMain.Clone();
                foreach (DataRow temp in foundRows2)
                {
                    table.ImportRow(temp);
                }

                double TotalAmountProposedForDivsion = 0.0;
                for (int u = 0; u < table.Rows.Count; u++)
                {
                    TotalAmountProposedForDivsion += double.Parse(table.Rows[u]["Proposed"].ToString());
                }

                Result += "<div style='margin-left:5px'><h1>Allocate Projects to Sections</h1></div>";// <tr>            <td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;' >Project Name</td>        <td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Proposed Percetage</td>   <td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;' >Approved Percetage</td> <td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Cost</td>            <td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Quantity</td>                   <td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Select</td>        </tr>";
                Result += "<table class='TFtable' border='1' style='width:90%;'><tr><td>Total Allocated Budget :<b> " + allocatedBudget + "</b></td></tr>   <tr>            <td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;' >Project Name</td>        <td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Budget</td>            <td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Quantity</td>                   <td style='font-size:15px;color: #FFFFFF; background-color: #3399FF;'>Select</td>        </tr>";
                for (int i = 0; i < table.Rows.Count; i++)
                {

                    double ProposedforThisProject = double.Parse(table.Rows[i]["Proposed"].ToString());
                    double TempVariable = ProposedforThisProject / TotalAmountProposedForDivsion * 100;

                    
                   // Result += "<tr>            <td style='font-size:18px'>" + table.Rows[i]["Project"].ToString() + "</td>            <td style='font-size:18px' >" + TempVariable + "</td><td style='font-size:18px'>" + TempVariable + "</td><td style='font-size:18px'>" + table.Rows[i]["Cost"].ToString() + "</td>            <td style='font-size:18px'>" + table.Rows[i]["Quantity"].ToString() + "</td>                                                      <td>             <input class='projects' type='checkbox' name='" + table.Rows[i]["ID"].ToString() + "' />     </td>        </tr>";
                     Result += "<tr>            <td style='font-size:18px'>" + table.Rows[i]["Project"].ToString() + "</td>           <td style='font-size:18px'>" + table.Rows[i]["Cost"].ToString() + "</td>            <td style='font-size:18px'>" + table.Rows[i]["Quantity"].ToString() + "</td>                                                      <td>             <input class='projects' type='checkbox' name='" + table.Rows[i]["ID"].ToString() + "' />     </td>        </tr>";
                }
                Result += "</table></center> <br>";
                string SectionsOfDivsion = GetSectionofDivision(Division);
                Result += " <h2>Selection Section </h2> " + SectionsOfDivsion;
                if (table.Rows.Count == 0)
                {
                    Result = "<h1>Project already Allocated</h1>";
                }
            }
            catch (Exception ex)
            {
                Result = ex.Message.ToString();
                //throw;
            }
            return Result;
        }
        public string GetSectionofDivision(string Division)
        {

            Division = Division.Trim().ToString().TrimEnd().ToString();
            string Result = string.Empty;
            try
            {
                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
                SP.List oList = clientContext.Web.Lists.GetByTitle("QCCChart");

                SP.CamlQuery camlQuery = new SP.CamlQuery();
                camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='Division'/>" +
                 "<Value Type='Text'>" + Division + "</Value></Eq></Where></Query></View>";
                SP.ListItemCollection collListItem = oList.GetItems(camlQuery);

                clientContext.Load(collListItem);

                clientContext.ExecuteQuery();

                DataTable table = new DataTable();
                table.Columns.Add("Section", typeof(string));
                table.Columns.Add("ID", typeof(string));

                foreach (SP.ListItem oListItem in collListItem)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = oListItem["Section"];
                    dr[1] = oListItem["ID"];
                    table.Rows.Add(dr);
                }

                Result += "<select id='section'>";
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    Result += "<option value=''>" + table.Rows[i]["Section"].ToString() + "</option>";
                   
                }
                Result += "</select><br>";
            }
            catch (Exception ex)
            {
                Result = ex.Message.ToString();
                //throw;
            }
            return Result;
        }
        #region Setting Sectors Data
        public string SetSectorQuota(string SectorsData, string Type, string Total)
        {
           // throw new Exception(SectorsData);
            string REsult = "Successfully Allocated Amount to Division";

            try
            {
                string[] DivisionParser = SectorsData.Split('|');
                for (int i = 0; i < DivisionParser.Length - 1; i++)
                {
                    string[] DataParser = DivisionParser[i].ToString().Split(':');
                    string SectorName = DataParser[0];
                    string Amount = DataParser[1];
                    double AmountTemp = 0.0;
                    double TotalAmount = double.Parse(Total);

                    if (Type == "Percentage" && Amount != null)
                    {
                        double temp = double.Parse(Amount);
                        AmountTemp = TotalAmount * temp / 100;
                    }
                    else
                    {
                        AmountTemp = double.Parse(Amount);
                    }
                   
                    if (CheckItemIntheList(SectorName,AmountTemp.ToString(),Type) == "No")
                    {
                    }
                    else
                    {
                        SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
                        SP.List oList = clientContext.Web.Lists.GetByTitle("SectorsAllocatedBudget");
                        NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                        clientContext.Credentials = credentials;
                        ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                        ListItem oListItem = oList.AddItem(itemCreateInfo);
                        oListItem["SectorName"] = SectorName;
                        oListItem["AllocatedBudget"] = AmountTemp.ToString();
                        oListItem["approvedtype"] = Type;
                        oListItem.Update();
                        clientContext.ExecuteQuery();
                    }

                }


            }
            catch (Exception ex)
            {

                // throw new Exception("Error Contact Administrator");
                REsult = ex.Message.ToString() + SectorsData.ToString();
            }
            //return "Successfully Allocated Amount to Division";
            return REsult;
        }
        public string CheckItemIntheList(string ItemName,string Amount,string Types)
        {
            string Result = string.Empty;
            try
            {
                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
                SP.List oList = clientContext.Web.Lists.GetByTitle("SectorsAllocatedBudget");

                SP.CamlQuery camlQuery = new SP.CamlQuery();
                camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='SectorName'/>" +
                 "<Value Type='Text'>" + ItemName + "</Value></Eq></Where></Query></View>";
                SP.ListItemCollection collListItem = oList.GetItems(camlQuery);

                clientContext.Load(collListItem);

                clientContext.ExecuteQuery();

                DataTable table = new DataTable();
                table.Columns.Add("SectorName", typeof(string));
                table.Columns.Add("ID", typeof(string));
                foreach (SP.ListItem oListItem in collListItem)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = oListItem["SectorName"];
                    dr[1] = oListItem["ID"];
                    table.Rows.Add(dr);
                }
                if (table.Rows.Count > 0)
                {
                    Result = "No";
                    UpdateAllocatedBudgetForSectors(table.Rows[0]["ID"].ToString(), ItemName, Amount, Types);
                }
                else
                {
                    Result = "Yes";
                }
            }
            catch (Exception ex)
            {
                Result = ex.Message.ToString();
            }
            return Result; 

        }
        public void UpdateAllocatedBudgetForSectors(string ID, string Sec,string AllocatedAmount,string Type)
        {
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
            NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
            clientContext.Credentials = credentials;
            List oList = clientContext.Web.Lists.GetByTitle("SectorsAllocatedBudget");
            ListItem oListItem = oList.GetItemById(ID);
            oListItem["SectorName"] = Sec;// "100";
            oListItem["approvedtype"] = Type;// "100";
            oListItem["AllocatedBudget"] = AllocatedAmount;// "100"; 
            oListItem.Update();
            clientContext.ExecuteQuery();

        }
        #endregion
        #region SettingDivisoin Qouta
        public string CheckDivisionFoundIntheList(string sector, string Division, string Amount, string Types)
        {
            string Result = string.Empty;
            try
            {
                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
                SP.List oList = clientContext.Web.Lists.GetByTitle("DivisionAllocatedBudget");

                SP.CamlQuery camlQuery = new SP.CamlQuery();
                camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='Division'/>" +
                 "<Value Type='Text'>" + Division + "</Value></Eq></Where></Query></View>";
                SP.ListItemCollection collListItem = oList.GetItems(camlQuery);

                clientContext.Load(collListItem);

                clientContext.ExecuteQuery();

                DataTable table = new DataTable();
                table.Columns.Add("Division", typeof(string));
                table.Columns.Add("ID", typeof(string));
                foreach (SP.ListItem oListItem in collListItem)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = oListItem["Division"];
                    dr[1] = oListItem["ID"];
                    table.Rows.Add(dr);
                }

                if (table.Rows.Count > 0)
                {
                    Result = "No";
                    UpdateAllocatedBudgetForDivision(table.Rows[0]["ID"].ToString(), sector, Division, Amount, Types);

                }
                else
                {
                    Result = "Yes";
                }
            }
            catch (Exception ex)
            {
                Result = ex.Message.ToString();
            }
            return Result;
        }
        public void UpdateAllocatedBudgetForDivision(string ID, string Sector, string Division, string AllocatedAmount, string Types)
        {
            try
            {
                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;
                List oList = clientContext.Web.Lists.GetByTitle("DivisionAllocatedBudget");
                ListItem oListItem = oList.GetItemById(ID);
                oListItem["Sector"] = Sector;// "100";
                oListItem["Division"] = Division;// "100";
                oListItem["AmountAllocated"] = AllocatedAmount;// "100";
                oListItem["Types"] = Types;// "100";
                oListItem.Update();
                clientContext.ExecuteQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        public string FetchSectorsofQCC(string Signature)
        {
            string Result = "";
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
            SP.List oList = clientContext.Web.Lists.GetByTitle("SectorsAllocatedBudget");
            SP.CamlQuery camlQuery = new SP.CamlQuery();
            camlQuery.ViewXml = "<View><Query><Where><Gt><FieldRef Name='ID'/>" + "<Value Type='Counter'>1</Value></Gt></Where></Query></View>";
            SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();
            DataTable table = new DataTable();
            table.Columns.Add("SectorName", typeof(string));
            table.Columns.Add("AllocatedBudget", typeof(string));
            double TotalAmount = 0.0;
            foreach (SP.ListItem oListItem in collListItem)
            {
                DataRow dr = table.NewRow();
                dr[0] = oListItem["SectorName"];
                dr[1] = oListItem["AllocatedBudget"];

                table.Rows.Add(dr);
            }

            for (int x = 0; x < table.Rows.Count; x++)
            {
                if (table.Rows[x]["AllocatedBudget"].ToString() != string.Empty)
                {
                    double ParseTemp = double.Parse(table.Rows[x]["AllocatedBudget"].ToString());
                    TotalAmount += ParseTemp;
                }
                Result += table.Rows[x]["SectorName"].ToString() + ":" + table.Rows[x]["AllocatedBudget"].ToString() + "|";
            }
            return Result + "_" + TotalAmount;
        }
        public string UpdateProjects(string section, string Data)
        {
            string Result = string.Empty;
            try
            {
                string[] SectionParse = Data.ToString().Split('|');
                for (int i = 0; i < SectionParse.Length-1; i++)
                {
                    UpdateProjectsForEachSection(SectionParse[i].ToString(), section);
                }
            }
            catch (Exception ex)
            {
                Result += ex.Message.ToString();
            }
            return Result;
        }
        public void UpdateProjectsForEachSection(string ID, string Section)
        {
            try
            {
                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;
                List oList = clientContext.Web.Lists.GetByTitle("Budget");
                ListItem oListItem = oList.GetItemById(ID);
                oListItem["Section"] = Section;// "100";
                oListItem.Update();
                clientContext.ExecuteQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public DataTable GetBudgetMainSector()
        {
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
            SP.List oList = clientContext.Web.Lists.GetByTitle("Budget");
            SP.CamlQuery camlQuery = new SP.CamlQuery();
            camlQuery.ViewXml = "<View><Query><Where><Gt><FieldRef Name='ID'/>" + "<Value Type='Counter'>1</Value></Gt></Where></Query></View>";
            SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("MainSector", typeof(string));
            table.Columns.Add("Total", typeof(string));
            table.Columns.Add("Project", typeof(string));
            table.Columns.Add("Proposed", typeof(string));
            foreach (SP.ListItem oListItem in collListItem)
            {
                DataRow dr = table.NewRow();
                dr[0] = oListItem["ID"];
                dr[1] = oListItem["MainSector"];
                dr[2] = oListItem["Total"];
                dr[3] = oListItem["Project"];
                dr[4] = oListItem["Proposed"];
                table.Rows.Add(dr);
            }
            return table;

        }
        public DataTable GetApprovedBudget_WithMainSector()
        {
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
            SP.List oList = clientContext.Web.Lists.GetByTitle("Budget");
            SP.CamlQuery camlQuery = new SP.CamlQuery();
            //camlQuery.ViewXml = "<View><Query><Where><And><Eq><FieldRef Name='Sector'/>" + "<Value Type='Text'>" + a + "</Value> </Eq><Eq><FieldRef Name='CAPEX'/>" + "<Value Type='Text'>" + b + "</Value></Eq></And></Where></Query><RowLimit>100</RowLimit></View>";
            camlQuery.ViewXml = "<View><Query><Where><Gt><FieldRef Name='ID'/>" + "<Value Type='Counter'>1</Value></Gt></Where></Query></View>";
            SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("MainSector", typeof(string));
            table.Columns.Add("Total", typeof(string));
            table.Columns.Add("Project", typeof(string));
            table.Columns.Add("Proposed", typeof(string));
            foreach (SP.ListItem oListItem in collListItem)
            {
                DataRow dr = table.NewRow();
                dr[0] = oListItem["ID"];
                dr[1] = oListItem["MainSector"];
                dr[2] = oListItem["Total"];
                dr[3] = oListItem["Project"];
                dr[4] = oListItem["Proposed"];
                table.Rows.Add(dr);
            }
            return table;

        }
        public string GetProjectBasedOnUserLogin(string Sector, string Division, string Section)
        {
           // System.IO.StreamWriter _Wr = new System.IO.StreamWriter("C:\\tmp\\LogError.txt",true);
            //_Wr.WriteLine(Sector);
            
            

            
            string Result = "";// Sector + Division + Section; //string.Empty;
            
            string TempSector = Sector.TrimEnd(' ').ToString();
           // Result += TempSector.Length;

          //  TempSector = "Corporate Support Services";
            string Caml = "";
            try
            {
                
                Caml = "<View><Query><Where><Eq><FieldRef Name='MainSector'/>'" + "<Value Type='Text'>" + TempSector + "</Value></Eq></Where></Query></View>";
                //Caml = "<View><Query><Where><Gt><FieldRef Name='ID'/>'" + "<Value Type='Counter'>0</Value></Gt></Where></Query></View>";
                
                
                // Result +=Caml;
                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/FinancenBudgets/");
                SP.List oList = clientContext.Web.Lists.GetByTitle("Budget");

                SP.CamlQuery camlQuery = new SP.CamlQuery();
                camlQuery.ViewXml = Caml;
                SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
                clientContext.Load(collListItem);
                clientContext.ExecuteQuery();
                
                
                Result += "<option value='0000000'>Select Project</option>";
                foreach (SP.ListItem oListItem in collListItem)
                {
                    if (Convert.ToString(oListItem["Stage"]) == "2")
                    {
                        Result += "<option value='" + oListItem["ID"] + "'>" + oListItem["Project"].ToString() + "</option>";
                    }
                }
            }
            catch (Exception ex)
            {

                //throw new Exception("Contact Support");

                //_Wr.WriteLine(ex.Message.ToString());
            }
            finally
            {
                
                
            }

            return Result;
        }
        public string AddingCarsRequest(string Applicant, string Typed, string Employee, string Fromdate, string todate, string mobilenumber, string Reason, string talentid, string location,
        string Section, string useremail,string Instrument)
        {
            string Result = string.Empty;
            try
            {
                ClientContext clientContext = new ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/QCC%20Cars/");
                SP.List oList = clientContext.Web.Lists.GetByTitle("CarsRequest");
                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;
                ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                ListItem oListItem = oList.AddItem(itemCreateInfo);
                oListItem["Title"] = "QCC Cars Management";
                oListItem["Sector"] = "Sector";
                oListItem["Division"] = "Division";//Cars.CarNumberPlate;
                oListItem["Section"] = Section;
                oListItem["Applicant"] = 
                oListItem["Typed"] = Typed;
                oListItem["NumberPlate"] = "1";/// numbers ofcars
                oListItem["Employee"] = Employee;
                oListItem["Fromdate"] = Fromdate;
                oListItem["todate"] = todate;
                oListItem["mobilenumber"] = mobilenumber;
                oListItem["Status"] = "100";
                oListItem["talentid"] = talentid;
                oListItem["CarNumberPlate"] = "123";
                oListItem["Location"] = location;
                oListItem["applicantEmail"] = useremail;
                oListItem["InstrumentInCar"] = Instrument;

                oListItem.Update();
                clientContext.ExecuteQuery();
                Result = " Successfuly added your Request";
            }
            catch (Exception ex)
            { Result = ex.Message.ToString(); }
            return Result;
        }
        public string MeetingRoomRequest(string Sector, string Division, string Section, string Applicant, string Attendies, string Catring, string FromDate, string NumberofAttendies, string Remarks, string Room, string ToDate)
        {
            string Result = string.Empty;
            try
            {
                ClientContext clientContext = new ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/QCC%20Cars/");
                SP.List oList = clientContext.Web.Lists.GetByTitle("MeetingRequest");
                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;
                ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                ListItem oListItem = oList.AddItem(itemCreateInfo);
                //DateTime dt = DateTime.Parse(FromDate).ToUniversalTime();

                //DateTime dt1 = DateTime.Parse(ToDate).ToUniversalTime();


                oListItem["Title"] = "QCC Meeting Request Management";
                oListItem["Sector"] = Sector;
                oListItem["Division"] = Division;
                oListItem["Section"] = Section;
                oListItem["Applicant"] = Applicant;
                oListItem["Attendies"] = Attendies;
                oListItem["Catring"] = Catring;/// numbers ofcars
                oListItem["FromDate"] = FromDate;// FromDate;
                oListItem["NumberofAttendies"] = NumberofAttendies;
                oListItem["Remarks"] = Remarks;
                oListItem["Room"] = Room;
                oListItem["ToDate"] = ToDate;// ToDate;
                oListItem.Update();
                clientContext.ExecuteQuery();
                Result = " Successfuly added your Request";
            }
            catch (Exception ex)
            { Result = ex.Message.ToString(); }
            return Result;
        }
        public string CatringRequest(string Sector, string Division, string Section, string Applicant, string Attendees, string Coordinator, string FromDate, string MobileNumber, string NumberofAttendies, string Remarks, string Todate, string RequestedBy, string CatringType)
        {
            string Result = string.Empty;
            try
            {
                ClientContext clientContext = new ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/QCC%20Cars/");
                SP.List oList = clientContext.Web.Lists.GetByTitle("Catering");
                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;
                ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                ListItem oListItem = oList.AddItem(itemCreateInfo);
                DateTime dt = DateTime.Parse(FromDate).ToUniversalTime();
                DateTime dt1 = DateTime.Parse(Todate).ToUniversalTime();
                oListItem["Title"] = "QCC Catering Request Management";
                oListItem["Sector"] = Sector;
                oListItem["Division"] = Division;
                oListItem["Section"] = Section;
                oListItem["Applicant"] = Applicant;
                oListItem["Attendees"] = Attendees;
                oListItem["Coordinator"] = Coordinator;/// numbers ofcars
                oListItem["FromDate"] = dt;// FromDate;
                oListItem["NumberofAttendies"] = NumberofAttendies;
                oListItem["Remarks"] = Remarks;
                oListItem["MobileNumber"] = MobileNumber;
                oListItem["Todate"] = dt1;// ToDate;
                oListItem["RequestedBy"] = RequestedBy;// ToDate;
                oListItem["CatringType"] = CatringType;
                oListItem.Update();
                clientContext.ExecuteQuery();
                Result = " Successfuly added your Request";
            }
            catch (Exception ex)
            { Result = ex.Message.ToString(); }
            return Result;
        }
        public string GetExecutiveDirectoryCharts(string Sector)
        {

            string X = string.Empty;
            try
            {
                DataTable AprovedBudget = GetApprovedBudgetbysector2();
                DataTable Divisionc = GetDivisionofSector(Sector);
                DataView view = new DataView(Divisionc);
                DataTable distinctValues = view.ToTable(true, "Division");
                DataTable tablex = new DataTable();
                DataTable table = new DataTable();
                string Temp = Sector.ToString().Replace(" ", "").ToString().TrimEnd().ToString();
                string AllocatedBugettoThisSector = GetAllocatedBudgetBySectorOrDivision(Temp, "Sector");
                tablex.Columns.Add("ID", typeof(string));
                tablex.Columns.Add("Sector", typeof(string));
                tablex.Columns.Add("Total", typeof(string));
                tablex.Columns.Add("Project", typeof(string));
                tablex.Columns.Add("Proposed", typeof(string));
                tablex.Columns.Add("Division", typeof(string));

                DataRow[] foundRows2;
                foundRows2 = AprovedBudget.Select("Sector='" + Sector + "'");
                double SumofApprovedBudget = 0.0;
                double SumofApprovedBudgetProposed = 0.0;
                foreach (DataRow temp in foundRows2)
                {
                    tablex.ImportRow(temp);
                }


                for (int y = 0; y < tablex.Rows.Count; y++)
                {
                    //Approved
                    string TempSume = tablex.Rows[y]["Total"].ToString();
                    double temp2 = double.Parse(TempSume);
                    SumofApprovedBudget += temp2;

                    //Prpposed
                    string TempSumeProposed = tablex.Rows[y]["Proposed"].ToString();
                    double temp2Proposed = double.Parse(TempSumeProposed);
                    SumofApprovedBudgetProposed += temp2Proposed;


                }
                double Devi = SumofApprovedBudget - SumofApprovedBudgetProposed;
                double Perce = Devi / SumofApprovedBudget;
                Perce = Perce * 100;
                StringBuilder AnotherTable = new StringBuilder();
                double _DivisoinsAmountproposed = 0.0;
                double _DivisoinsAmount = 0.0;
                string DiviName = string.Empty;
                string DiviName1 = string.Empty;
                for (int i = 0; i < distinctValues.Rows.Count; i++)
                {
                    if (distinctValues.Rows[i]["Division"].ToString().ToLower().ToString() != "none")
                    {
                        _DivisoinsAmount = 0.0;
                        _DivisoinsAmountproposed = 0.0;
                        for (int z = 0; z < tablex.Rows.Count; z++)
                        {
                            if (tablex.Rows[z]["Division"].ToString() == distinctValues.Rows[i]["Division"].ToString())
                            {
                                _DivisoinsAmount += double.Parse(tablex.Rows[z]["Total"].ToString());
                                _DivisoinsAmountproposed += double.Parse(tablex.Rows[z]["Proposed"].ToString());
                            }
                        }
                        DiviName = distinctValues.Rows[i]["Division"].ToString().Trim().ToString().Replace(" ", "").ToString();
                        DiviName1 += distinctValues.Rows[i]["Division"].ToString().Trim().ToString().Replace(" ", "").ToString() + "|";
                        double ProposedPercentageDivision = 0.0;
                        double approvedPercentageDivision = 0.0;


                        double divpercentage = _DivisoinsAmountproposed / SumofApprovedBudgetProposed;
                        double temppercentage = divpercentage * 100;
                        // temppercentage = temppercentage *100;


                        double divpercentageappr = _DivisoinsAmount / SumofApprovedBudget - _DivisoinsAmount;
                        double temppercentage3 = divpercentage * 100;
                        //temppercentage3 = temppercentage3 * 100;
                        if (temppercentage == 0)
                        {
                            temppercentage = 0.0;
                        }

                        int Temp6 = Convert.ToInt32(temppercentage);
                        AnotherTable.Append(distinctValues.Rows[i]["Division"].ToString()); AnotherTable.Append("|");
                        AnotherTable.Append(Temp6.ToString());
                        AnotherTable.Append("_");
                    }
                }
                X = AnotherTable.ToString();
            }
            catch (Exception ex)
            {
                X += ex.Message.ToString();
            }
            return X;
        }
        public string UpdateScales(string BusinessCategory ,string CalculationType,
                                string CompanyId,string eval2,
                                string Maximum2,string QCCTagNumber,
                                string ScaleCategory,string ScaleClass,
                                string ScaleMiniMum,string ScaleRangeUsed,
                                string ScalVd,string ScalVd2,
                                string ScalVe,string ScManufacturer,
                                string ScMax,string ScMin,
                                string ScModel,string ScNumberofDisplay,
                                string ScSerialNo,string ScTypeApproval, string Id)
        {
            string Result = String.Empty;
            try
            {
                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/ConsumerAndMarketServices/MarketSurveillance/QCCMeters");
                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;
                List oList = clientContext.Web.Lists.GetByTitle("QCCScales");
                ListItem oListItem = oList.GetItemById(Convert.ToInt32(Id));
                oListItem["BusinessCategory"] = BusinessCategory;
                oListItem["CalculationType"] = CalculationType;
                oListItem["CompanyId"] = CompanyId;
                oListItem["eval2"] = eval2;
                oListItem["Maximum2"] = Maximum2;
                oListItem["QCCTagNumber"] = QCCTagNumber;
                oListItem["ScaleCategory"] = ScaleCategory;
                oListItem["ScaleClass"] = ScaleClass;
                oListItem["ScaleMiniMum"] = ScaleMiniMum;
                oListItem["ScaleRangeUsed"] = ScaleRangeUsed;
                oListItem["ScalVd"] = ScalVd;
                oListItem["ScalVd2"] = ScalVd2;
                oListItem["ScalVe"] = ScalVe;
                oListItem["ScManufacturer"] = ScManufacturer;
                oListItem["ScMax"] = ScMax;
                oListItem["ScMin"] = ScMin;
                oListItem["ScModel"] = ScModel;
                oListItem["ScNumberofDisplay"] = ScNumberofDisplay;
                oListItem["ScSerialNo"] = ScSerialNo;
                oListItem["ScTypeApproval"] = ScTypeApproval;


                oListItem.Update();
                clientContext.ExecuteQuery();
            }
            catch (Exception ex )
            {

                Result += ex.Message.ToString();
            }
            return Result;
        }


        public string NewPaperRequest(string Application_Type, string Division, string NewsPaperCompany, string Sector, string Section, string Applicant, string Requestedby)
        {
            string Result = String.Empty;
            try
            {
                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/QCC%20Cars");
                List oList = clientContext.Web.Lists.GetByTitle("NewsPaper");
                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;
                ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                ListItem oListItem = oList.AddItem(itemCreateInfo);
                oListItem["Title"] = "QCC Newpaper Request ";
                oListItem["NewPaperName"] = Application_Type;
                oListItem["Division"] = Division;
                oListItem["Section"] = Section;
                oListItem["ApplicantName"] = Applicant;
                oListItem["Sector"] = Sector;/// numbers ofcars
                oListItem["Author"] = Requestedby;
                oListItem.Update();
                clientContext.ExecuteQuery();
                Result = " Successfuly added your Request";
            }
            catch (Exception ex)
            {

                Result += ex.Message.ToString();
            }
            return Result;
        }

        public string ParkingRequest(string Sector, string Division, string Section, string TalantName, string Date, string CarType, string NumberPlat, string Requestedby,string ParkingDate)
        {
            string Result = String.Empty;
            try
            {
                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/QCC%20Cars");
                List oList = clientContext.Web.Lists.GetByTitle("ParkingRequest");
                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;
                ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                ListItem oListItem = oList.AddItem(itemCreateInfo);
                oListItem["Title"] = "QCC Parking Request ";
                oListItem["Sector"] = Sector;
                oListItem["Division"] = Division;
                oListItem["Section"] = Section;
                oListItem["TalantName"] = TalantName;
                oListItem["CarType"] = CarType;/// numbers ofcars
                oListItem["NumberPlat"] = NumberPlat;
                oListItem["Requestedby"] = Requestedby;
                oListItem["ParkingDate"] = ParkingDate;
                oListItem.Update();
                clientContext.ExecuteQuery();
                Result = " Successfuly added your Request";
            }
            catch (Exception ex)
            {

                Result += ex.Message.ToString();
            }
            return Result;
        }

        public string CheckMeetingRoomStatus(string FromDate, string Todate)
        {
            string Result = "";// "from Date" + FromDate + "Todate" + Todate;
            
            try 
	        {
                DateTime dt = DateTime.Parse(FromDate);//.ToUniversalTime();
                DateTime dt1 = DateTime.Parse(Todate);//.ToUniversalTime();
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/QCC Cars");
            SP.List oList = clientContext.Web.Lists.GetByTitle("MeetingRequest");
            SP.CamlQuery camlQuery = new SP.CamlQuery();
            camlQuery.ViewXml = "<View><Query><Where><Gt><FieldRef Name='ID'/>" + "<Value Type='Counter'>1</Value></Gt></Where></Query></View>";
            SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("FromDate", typeof(string));
            table.Columns.Add("ToDate", typeof(string));
            foreach (SP.ListItem oListItem in collListItem)
            {
                DataRow dr = table.NewRow();
                dr[0] = oListItem["ID"];
                string D = oListItem["FromDate"].ToString();
                string D1 = oListItem["ToDate"].ToString();
                
                DateTime x= DateTime.Parse(D);
                DateTime y = DateTime.Parse(D1);
                dr[1] = x;
                dr[2] = y;
                table.Rows.Add(dr);
            }

            string filter = "FromDate ='" + dt + "' AND ToDate = '" + dt1 + "'";
            DataTable X = new DataTable();

            DataRow[] Xrow = table.Select(filter);
            foreach (DataRow temp in Xrow)
            {
                X.ImportRow(temp);
            }
            if (X.Rows.Count > 0)
            {
                Result = "1";
            }
            else
            {
                Result = "0";
            }

	        }
	        catch (Exception ex)
	        {

                Result += ex.Message.ToString();

	        }
            return Result;
        }


        public string BringAllInventoryProduct(string product)
        {
            string Prodcut = "<option value='0'></option>";
            string ProdcutType = "<option value='0'></option>";
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/EServices/InventoryMgmt");
            Microsoft.SharePoint.Client.List spList = clientContext.Web.Lists.GetByTitle("QCCInventory");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();

            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new Microsoft.SharePoint.Client.CamlQuery();
                camlQuery.ViewXml =
                   @"<View>  
             <ViewFields><FieldRef Name='InventoryName' /><FieldRef Name='AssetTypes' /><FieldRef Name='QuantityUnits' /></ViewFields> 
                 </View>";
                Microsoft.SharePoint.Client.ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();
                List<string> _ls = new List<string>();
                foreach (SP.ListItem oListItem in listItems)
                {

                    if (oListItem["AssetTypes"] != null)
                    {
                        if (!_ls.Contains(oListItem["AssetTypes"].ToString()))
                        {
                            _ls.Add(oListItem["AssetTypes"].ToString());
                            ProdcutType += "<option value=" + oListItem["QuantityUnits"] + " >" + oListItem["AssetTypes"] + "</option>";
                        }
                    }

                }
            }
            return ProdcutType;

        }





        #region EMC

        public string SendToSG(string TopicIds)
        {
            string res = "Done";
            try
            {

                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/GS/EMC/");
                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;

                string[] topics=TopicIds.Split('|');

                for (int i=0;i<topics.Length-1;i++)
                {
                    List oList = clientContext.Web.Lists.GetByTitle("MeetingRequest");
                    ListItem oListItem = oList.GetItemById(Convert.ToInt32(topics[i]));
                    oListItem["AssignedTo"] = "SG View";// "100";
                    oListItem.Update();
                    clientContext.ExecuteQuery();
                    res += topics[i]+"from server";
                }

                //Email send to SG
                ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                List oListEmailSG = clientContext.Web.Lists.GetByTitle("EmailSG");
                ListItem oListItemEmailSG = oListEmailSG.AddItem(itemCreateInfo);
                oListItemEmailSG["Title"] = "Email";

                oListItemEmailSG.Update();
                clientContext.ExecuteQuery();


            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string SendBackToED(string TopicID)
        {
            string res="Done";
            try 
	    {	        
		
            SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/GS/EMC/");
            NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
            clientContext.Credentials = credentials;
            List oList = clientContext.Web.Lists.GetByTitle("MeetingRequest");
            ListItem oListItem = oList.GetItemById(Convert.ToInt32(TopicID));
            oListItem["AssignedTo"] = "ED";// "100";
            oListItem.Update();
            clientContext.ExecuteQuery();
                
	    }
	    catch (Exception ex)
	    {
            res=ex.Message;
	    }
            return res;
        }
        public string getEMC()
        {
            StringBuilder resultHTML=new StringBuilder() ;
            string meetingTypeDropdown="";
            string allTopicIds = "";
            try
            {
                SP.ClientContext clientContextEMC = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/GS/EMC/");//"");
                SP.List oList = clientContextEMC.Web.Lists.GetByTitle("MeetingRequest");


                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContextEMC.Credentials = credentials;
                SP.CamlQuery camlQuery = new SP.CamlQuery();
                camlQuery.ViewXml = "<View/>";

                SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
                clientContextEMC.Load(collListItem);

                clientContextEMC.Load(collListItem,
                 items => items.Include(
                    item => item["Id"],
                    item => item["Sector"],
                    item => item["Topic"],
                    item => item["Meeting_x0020_Type"],
                    item => item["Catagory"],
                     item => item["SubCatagory"],
                    item => item["Status"],
                    item => item["Importance"],
                    item => item["TopicDescription"],
                    item => item["field3"],
                    item => item["remainDuration"],
                    item => item["AssignedTo"],
                    item => item["MeetingDate"]    
                    ));
                clientContextEMC.ExecuteQuery();

                DataTable table = new DataTable();
                table.Columns.Add("ID", typeof(string));
                table.Columns.Add("Sector", typeof(string));
                table.Columns.Add("Topic", typeof(string));
                table.Columns.Add("Meeting_x0020_Type", typeof(string));
                table.Columns.Add("Catagory", typeof(string));
                table.Columns.Add("SubCatagory", typeof(string));
                 table.Columns.Add("Status", typeof(string));
                table.Columns.Add("Importance", typeof(string));
                table.Columns.Add("TopicDescription", typeof(string));
                table.Columns.Add("field3", typeof(string));
                table.Columns.Add("remainDuration", typeof(string));
                table.Columns.Add("MeetingDate", typeof(DateTime));


                string meetingDate="";
                int count = 0;
                foreach (SP.ListItem oListItem in collListItem)
                {

                    if (Convert.ToString(oListItem["AssignedTo"])!="ED" && Convert.ToString(oListItem["AssignedTo"])!="SG View"
                        && Convert.ToString(oListItem["AssignedTo"]) == "NasirView" && Convert.ToDateTime(oListItem["MeetingDate"])>DateTime.Today)
                    {

                        if (oListItem["MeetingDate"]!=null)
                            meetingDate=Convert.ToString(Convert.ToDateTime(oListItem["MeetingDate"]).ToShortDateString());
                        DataRow dr = table.NewRow();
                        dr[0] = oListItem["ID"];
                        dr[1] = oListItem["Sector"];
                        dr[2] = oListItem["Topic"];
                        dr[3] = Convert.ToString(oListItem["Meeting_x0020_Type"]);
                        dr[4] = Convert.ToString(oListItem["Catagory"]);
                        dr[5] = Convert.ToString(oListItem["SubCatagory"]);
                        dr[6] = oListItem["Status"];
                        dr[7] = oListItem["Importance"];
                        dr[8] = oListItem["TopicDescription"];
                        dr[9] = oListItem["field3"];
                        dr[10] = oListItem["remainDuration"];
                        dr[11] = oListItem["MeetingDate"];
                        table.Rows.Add(dr);

                        allTopicIds+=Convert.ToString(oListItem["ID"])+"|";
                        count++;
                    }
                }


                DataView view = new DataView(table);
                DataTable distinctSectors = view.ToTable(true, "Sector");
                resultHTML.Append("<div><h1> Meeting Date - تاريخ الاجتماع : " + meetingDate + "</h1></div><br/><br/>");
                foreach (DataRow dr in distinctSectors.Rows)
                {
                    resultHTML.Append("<table class='gradienttable'><th colspan='3'><h1> " + dr["Sector"] + "</h1></th>");
                    DataRow[] resultTopics = table.Select("Sector='" + dr["Sector"] + "'");
                    resultHTML.Append("<tbody>");
                    foreach (DataRow dtrTopics in resultTopics)
                    {
                        resultHTML.Append("<tr>");
                        resultHTML.Append("<td class='1stColmn'>Topic</td><td>");
                        resultHTML.Append(dtrTopics["Topic"] + "</td><td class='1stColmn'>موضوع</td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'>Topic Type</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["Meeting_x0020_Type"] + "</td><td class='1stColmn'>نوع الموضوع</td></tr>");
                        //dtrTopics["Meeting_x0020_Type"]
                        resultHTML.Append("<tr><td class='1stColmn'>Priority</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["Catagory"] + "</td><td class='1stColmn'>أفضلية</td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'>Objective</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["SubCatagory"] + "</td><td class='1stColmn'>هدف</td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'>Status</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["Status"] + "</td><td class='1stColmn'>حالة</td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'>Importance</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["Importance"] + "</td><td class='1stColmn'>أهمية</td></tr>");



                        resultHTML.Append("<tr><td class='1stColmn'>Topic Description</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["TopicDescription"]);
                        resultHTML.Append("</td><td class='1stColmn'>وصف الموضوع</td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'>Topic Duration</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["field3"]);
                        resultHTML.Append(" Minutes</td><td class='1stColmn'>مدة الموضوع</td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'></td><td colspan='2' class='2ndColumn'><input type='button' value='Send Back - إعادة' style='float:right'  id='sent-" + dtrTopics["ID"] + "' onclick='topicSentBack(" + dtrTopics["ID"] + ")'>");
                        resultHTML.Append("<input type='button' value='Edit - تحرير' style='float:right' id='edit-" + dtrTopics["ID"] + "' onclick='topicEdit(" + dtrTopics["ID"] + ")'>");
                        resultHTML.Append("</td></tr>");

                        resultHTML.Append("<tr colspan='2' style='height: 25px;width: 100%;border-top: 1px solid #000;'>  </tr>");                        
                        
                    }
                    resultHTML.Append("</tbody>");
                    resultHTML.Append( "</table>");
                    resultHTML.Append("<span style='display:none' id='listTopics'>" + allTopicIds + "</span>");
 

                  
                }

                if (count == 0)
                {
                    resultHTML.Remove(0, resultHTML.Length );
                    resultHTML.Insert(0, "No agenda topics available");
                }
                else
                {
                    resultHTML.Append("<input type='button' style='float:right; marign-right:10px' value='Send to SG - رسال الى الأمين العام' onclick='sendToSG()'>");
                }

            }
            catch (Exception ex)
            {
                resultHTML.Append("Excp "+ ex.Message);
            }
                return resultHTML.ToString();


        }
       
        public string getEMCForSG()
        {
            StringBuilder resultHTML = new StringBuilder();
            string meetingTypeDropdown = "";
            string allTopicIds = "";
            try
            {
                SP.ClientContext clientContextEMC = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/GS/EMC/");//"");
                SP.List oList = clientContextEMC.Web.Lists.GetByTitle("MeetingRequest");


                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContextEMC.Credentials = credentials;
                SP.CamlQuery camlQuery = new SP.CamlQuery();
                camlQuery.ViewXml = "<View/>";

                SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
                clientContextEMC.Load(collListItem);

                clientContextEMC.Load(collListItem,
                 items => items.Include(
                    item => item["Id"],
                    item => item["Sector"],
                    item => item["Topic"],
                    item => item["Meeting_x0020_Type"],
                    item => item["Catagory"],
                     item => item["SubCatagory"],
                    item => item["Status"],
                    item => item["Importance"],
                    item => item["TopicDescription"],
                    item => item["field3"],
                    item => item["remainDuration"],
                    item => item["AssignedTo"],
                    item => item["MeetingDate"]
                    ));
                clientContextEMC.ExecuteQuery();

                DataTable table = new DataTable();
                table.Columns.Add("ID", typeof(string));
                table.Columns.Add("Sector", typeof(string));
                table.Columns.Add("Topic", typeof(string));
                table.Columns.Add("Meeting_x0020_Type", typeof(string));
                table.Columns.Add("Catagory", typeof(string));
                table.Columns.Add("SubCatagory", typeof(string));
                table.Columns.Add("Status", typeof(string));
                table.Columns.Add("Importance", typeof(string));
                table.Columns.Add("TopicDescription", typeof(string));
                table.Columns.Add("field3", typeof(string));
                table.Columns.Add("remainDuration", typeof(string));
                table.Columns.Add("MeetingDate", typeof(DateTime));

                int count = 0;
                string meetingDate = "";
                foreach (SP.ListItem oListItem in collListItem)
                {

                    if (Convert.ToString(oListItem["AssignedTo"]) == "SG View" && Convert.ToDateTime(oListItem["MeetingDate"])>DateTime.Today
                        )
                    {

                        if (oListItem["MeetingDate"] != null)
                            meetingDate = Convert.ToString(Convert.ToDateTime(oListItem["MeetingDate"]).ToShortDateString());

                        DataRow dr = table.NewRow();
                        dr[0] = oListItem["ID"];
                        dr[1] = oListItem["Sector"];
                        dr[2] = oListItem["Topic"];
                        dr[3] = Convert.ToString(oListItem["Meeting_x0020_Type"]);
                        dr[4] = Convert.ToString(oListItem["Catagory"]);
                        dr[5] = Convert.ToString(oListItem["SubCatagory"]);
                        dr[6] = oListItem["Status"];
                        dr[7] = oListItem["Importance"];
                        dr[8] = oListItem["TopicDescription"];
                        dr[9] = oListItem["field3"];
                        dr[10] = oListItem["remainDuration"];
                        dr[11]=oListItem["MeetingDate"];
                        table.Rows.Add(dr);

                        allTopicIds += Convert.ToString(oListItem["ID"]) + "|";
                        count++;
                    }
                }


                DataView view = new DataView(table);
                DataTable distinctSectors = view.ToTable(true, "Sector");
                resultHTML.Append("<div><h1> Meeting Date - تاريخ الاجتماع: " + meetingDate + "</h1></div><br/><br/>");
                foreach (DataRow dr in distinctSectors.Rows)
                {
                    resultHTML.Append("<table class='gradienttable'><th colspan='3'><h1>" + dr["Sector"] + "</h1></th>");
                    DataRow[] resultTopics = table.Select("Sector='" + dr["Sector"] + "'");
                    resultHTML.Append("<tbody>");
                    foreach (DataRow dtrTopics in resultTopics)
                    {
                        resultHTML.Append("<tr>");
                        resultHTML.Append("<td class='1stColmn'>Topic</td><td>");
                        resultHTML.Append(dtrTopics["Topic"] + "</td><td class='1stColmn'>موضوع</td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'>Topic Type</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["Meeting_x0020_Type"] + "</td><td class='1stColmn'>نوع الموضوع</td></tr>");
                        //dtrTopics["Meeting_x0020_Type"]
                        resultHTML.Append("<tr><td class='1stColmn'>Priority</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["Catagory"] + "</td><td class='1stColmn'>أفضلية</td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'>Objective</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["SubCatagory"] + "</td><td class='1stColmn'>هدف</td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'>Status</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["Status"] + "</td><td class='1stColmn'>حالة</td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'>Importance</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["Importance"] + "</td><td class='1stColmn'>أهمية</td></tr>");



                        resultHTML.Append("<tr><td class='1stColmn'>Topic Description</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["TopicDescription"]);
                        resultHTML.Append("</td><td class='1stColmn'>وصف الموضوع </td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'>Topic Duration</td><td class='2ndColumn'>");
                        resultHTML.Append(dtrTopics["field3"]);
                        resultHTML.Append(" Minutes</td><td class='1stColmn'>مدة الموضوع</td></tr>");

                        resultHTML.Append("<tr><td class='1stColmn'></td><td class='2ndColumn'colspan='2'><input type='button' style='float:right' value='Send Back - إعادة'  id='sent-" + dtrTopics["ID"] + "' onclick='topicSentBack(" + dtrTopics["ID"] + ")'>");
                        resultHTML.Append("<input type='button' value='Edit - تحرير' style='float:right' id='edit-" + dtrTopics["ID"] + "' onclick='topicEdit(" + dtrTopics["ID"] + ")'>");
                        resultHTML.Append("</td></tr>");

                        resultHTML.Append("<tr colspan='2' style='height: 25px;width: 100%;border-top: 1px solid #000;'>  </tr>");

                    }
                    resultHTML.Append("</tbody>");
                    resultHTML.Append("</table>");
                    resultHTML.Append("<span style='display:none' id='listTopics'>" + allTopicIds + "</span>");
                    
                }

                if (count == 0)
                {
                    resultHTML.Remove(0, resultHTML.Length - 1);
                    resultHTML.Insert(0, "No agenda topics available");
                }
                else
                {
                    resultHTML.Append("<input type='button' value='Approve All- الموافقة على جميع' style='float:right;maright-right:10px;' onclick='ApproveAll()'>");
                }

            }
            catch (Exception ex)
            {
                resultHTML.Append("Excp " + ex.Message);
            }
            return resultHTML.ToString();


        }
        public string ApproveAll(string TopicIds)
        {
            string res = "Done";
            try
            {

                SP.ClientContext clientContext = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/GS/EMC/");
                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContext.Credentials = credentials;

                string[] topics = TopicIds.Split('|');

                for (int i = 0; i < topics.Length - 1; i++)
                {
                    List oList = clientContext.Web.Lists.GetByTitle("MeetingRequest");
                    ListItem oListItem = oList.GetItemById(Convert.ToInt32(topics[i]));
                    oListItem["AssignedTo"] = "SG View";// "100";
                    oListItem["Approved"] = true;
                    oListItem.Update();
                    clientContext.ExecuteQuery();
                    res += topics[i] + "from server";
                }


             
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }


            return res;
        }
        public string getEMCDates(string empty)
        {
            StringBuilder resultHTML = new StringBuilder();
            
            try
            {
                SP.ClientContext clientContextEMC = new SP.ClientContext("http://intranet/sites/English/Sectors/CorporateSupport/SharedServices/GS/EMC/");//"");
                SP.List oList = clientContextEMC.Web.Lists.GetByTitle("MeetingRequest");


                NetworkCredential credentials = new NetworkCredential("bot1", "testing8#", "ADQCC");
                clientContextEMC.Credentials = credentials;
                SP.CamlQuery camlQuery = new SP.CamlQuery();
                camlQuery.ViewXml = "<View/>";

                SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
                clientContextEMC.Load(collListItem);

                clientContextEMC.Load(collListItem,
                 items => items.Include(

                    item => item["MeetingDate"]
                    ));
                clientContextEMC.ExecuteQuery();


                DataTable tblDates = new DataTable();

                tblDates.Columns.Add("MeetingDate", typeof(string));
                foreach (SP.ListItem oListItem in collListItem)
                {
                    DataRow dr = tblDates.NewRow();
                    dr[0] = DateTime.Parse(Convert.ToString(oListItem["MeetingDate"])).ToShortDateString();
                    
                    tblDates.Rows.Add(dr);
                }
                DataView dtview = new DataView(tblDates);
                DataTable dtdistinct = dtview.ToTable(true, "MeetingDate");
                 
                int count = 0;
                for (int i = 0; i < dtdistinct.Rows.Count;i++ )
                {
                    resultHTML.Append(dtdistinct.Rows[i]["MeetingDate"] + "|");


                    count++;
                }

                
                
                if (count == 0)
                {
                    resultHTML.Remove(0, resultHTML.Length);
                    resultHTML.Insert(0, "No agendas available");
                }
                

            }
            catch (Exception ex)
            {
                resultHTML.Append("Excp " + ex.Message);
            }
            return resultHTML.ToString();


        }
        
        //public void StartWorkflow(ListItem listItem, Site spSite, string wfName)
        //{
        //    List parentList = listItem.ParentList;
        //    WorkflowAssociationCollection associationCollection = parentList.WorkflowAssociations;
        //    foreach (WorkflowAssociation association in associationCollection)
        //    {
        //        if (association.Name == wfName)
        //        {
        //            association.AutoStartChange = true;
        //            association.AutoStartCreate = false;
        //            association.AssociationData = string.Empty;
        //            spSite.WorkflowManager.StartWorkflow(listItem, association, association.AssociationData);
        //        }
        //    }
        //}

        #endregion


        public string getApprovedEMC(string meetingDate)
        {
            throw new NotImplementedException();
        }

        void IQCCSystem.DoWork()
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.GetData(int value)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.GetInformation(string type, string fromdt, string todt)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.GetBudgetReports(string type)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.UpdateIteminformatoin(string Officer, string officerEmail, string ProjectType, string PurchasingType, string Delivrables, string QccStarategyMap, string ProcStartDate, string ID, string strtcatagories, string strtsubcat, string Divisions)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.GetExecutiveDirectory(string Sector)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.SetDivisionQuota(string Sector, string DivisionData, string Type, string Total)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.SetSectorQuota(string SectorsData, string Type, string Total)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.FetchSectorsofQCC(string Signature)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.GetDivisionLevelProjects(string Division)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.UpdateProjects(string section, string Data)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.GetProjectBasedOnUserLogin(string Sector, string Division, string Section)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.AddingCarsRequest(string Applicant, string Typed, string Employee, string Fromdate, string todate, string mobilenumber, string Reason, string talentid, string location, string Section, string useremail, string Instrument)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.MeetingRoomRequest(string Sector, string Division, string Section, string Applicant, string Attendies, string Catring, string FromDate, string NumberofAttendies, string Remarks, string Room, string ToDate)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.CatringRequest(string Sector, string Division, string Section, string Applicant, string Attendees, string Coordinator, string FromDate, string MobileNumber, string NumberofAttendies, string Remarks, string Todate, string RequestedBy, string CatringType)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.GetExecutiveDirectoryCharts(string Sector)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.SendToSG(string TopicIds)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.ApproveAll(string TopicIds)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.SendBackToED(string TopicID)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.getEMC()
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.getEMCForSG()
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.UpdateScales(string BusinessCategory, string CalculationType, string CompanyId, string eval2, string Maximum2, string QCCTagNumber, string ScaleCategory, string ScaleClass, string ScaleMiniMum, string ScaleRangeUsed, string ScalVd, string ScalVd2, string ScalVe, string ScManufacturer, string ScMax, string ScMin, string ScModel, string ScNumberofDisplay, string ScSerialNo, string ScTypeApproval, string Id)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.NewPaperRequest(string Application_Type, string Division, string NewsPaperCompany, string Sector, string Section, string Applicant, string Requestedby)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.getEMCDates(string empty)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.ParkingRequest(string Sector, string Division, string Section, string TalantName, string Date, string CarType, string NumberPlat, string Requestedby, string ParkingDate)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.CheckMeetingRoomStatus(string FromDate, string Todate)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.getApprovedEMC(string meetingDate)
        {
            throw new NotImplementedException();
        }

        string IQCCSystem.BringAllInventoryProduct(string product)
        {
            throw new NotImplementedException();
        }
    }
}
