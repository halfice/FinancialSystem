using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
namespace QCC_Market_Survellence
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IQCCSystem" in both code and config file together.
    [ServiceContract]
    public interface IQCCSystem
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
         ResponseFormat = WebMessageFormat.Json)]
        string GetData(int value);

          [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
         ResponseFormat = WebMessageFormat.Json)]
        string GetInformation(string type, string fromdt, string todt);

          [OperationContract]
          [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
          string GetBudgetReports(string type);


          [OperationContract]
          [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
          string UpdateIteminformatoin(string Officer, string officerEmail, string ProjectType, string PurchasingType, string Delivrables, string QccStarategyMap, string ProcStartDate, string ID, string strtcatagories, string strtsubcat,string Divisions);


          [OperationContract]
          [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
          string GetExecutiveDirectory(string Sector);


          [OperationContract]
          [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
          string SetDivisionQuota(string Sector,string DivisionData,string Type,string Total);

          [OperationContract]
          [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
          string SetSectorQuota(string SectorsData, string Type, string Total);

         [OperationContract]
          [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
          string FetchSectorsofQCC(string Signature);


         [OperationContract]
         [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
          ResponseFormat = WebMessageFormat.Json)]
         string GetDivisionLevelProjects(string Division);

        [OperationContract]
         [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
          ResponseFormat = WebMessageFormat.Json)]
         string UpdateProjects(string section, string Data);


          [OperationContract]
         [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
          ResponseFormat = WebMessageFormat.Json)]
          string GetProjectBasedOnUserLogin(string Sector, string Division, string Section);

          [OperationContract]
          [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
          string AddingCarsRequest(string Applicant ,string Typed ,string Employee , string Fromdate ,    string todate , string mobilenumber ,  string Reason , string talentid ,     string location ,
        string Section, string useremail, string Instrument);


          [OperationContract]
          [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
          string MeetingRoomRequest(string Sector, string Division, string Section, string Applicant, string Attendies, string Catring, string FromDate, string NumberofAttendies, string Remarks, string Room, string ToDate);

          [OperationContract]
          [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
          string CatringRequest(string Sector, string Division, string Section, string Applicant, string Attendees, string Coordinator, string FromDate, string MobileNumber, string NumberofAttendies, string Remarks, string Todate, string RequestedBy, string CatringType);

           [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
           string GetExecutiveDirectoryCharts(string Sector);

        

            [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
           string SendToSG(string TopicIds);

            [OperationContract]
            [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json)]
            string ApproveAll(string TopicIds);

           [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
            string SendBackToED(string TopicID);
        
           [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
           string getEMC();

           [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
           string getEMCForSG();

           [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
           string UpdateScales(string BusinessCategory ,string CalculationType,string CompanyId,string eval2,string Maximum2,string QCCTagNumber,string ScaleCategory,string ScaleClass,string ScaleMiniMum,string ScaleRangeUsed,string ScalVd,string ScalVd2,
           string ScalVe,string ScManufacturer,string ScMax,string ScMin,string ScModel,string ScNumberofDisplay,string ScSerialNo,string ScTypeApproval,string Id);


           [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
           string NewPaperRequest(string Application_Type, string Division, string NewsPaperCompany, string Sector, string Section, string Applicant, string Requestedby);

           [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
           string getEMCDates(string empty);


           [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
           string ParkingRequest(string Sector, string Division, string Section, string TalantName, string Date, string CarType, string NumberPlat, string Requestedby, string ParkingDate);

           [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
           string CheckMeetingRoomStatus(string FromDate, string Todate);

           [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
           string getApprovedEMC(string meetingDate);

        [OperationContract]
           [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
           ResponseFormat = WebMessageFormat.Json)]
           string BringAllInventoryProduct(string product);
        
        //

    }

    [DataContract]
    public class CarsRequest
    {
        [DataMember]
        public string Applicant { get; set; }
        [DataMember]
        public string Typed { get; set; }
        [DataMember]
        public string Employee { get; set; }
        [DataMember]
        public string Fromdate { get; set; }
        [DataMember]
        public string todate { get; set; }
        [DataMember]
        public string mobilenumber { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public string talentid { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string Section { get; set; }
        [DataMember]
        public string useremail { get; set; }


    }

    



    




}











