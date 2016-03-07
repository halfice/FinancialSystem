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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "INBAD_UtilityPaymentService" in both code and config file together.
    [ServiceContract]
    public interface INBAD_UtilityPaymentService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        string DoWork();

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        string Inquiry(string TradingLicenseNo,string ServiceID );

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        string Payment(string ServiceName, string TradingLicenseNo, string BankTransactionId);
       // string ServiceName,string TradingLicenseNo,
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        List<ADQCCReconcile> Reconcile(string ServiceName, string FromDate, string Todadate);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        List<ADQCCReconcile>[] ReconcileAll(string Keyword, string FromDate, string Todadate);


        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        void AddContentUsItem(string name, string telephoe, string email, string comment, string res);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        string Fun();



        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
        string Payments(string ServiceName, string TradingLicenseNo, string BankTransactionId, double Amount);

    }

    [DataContract]
    public class ADQCCReconcile
    {

        [DataMember]
        public string Status { get; set; }
          [DataMember]
        public string Error { get; set; }
          
          [DataMember]
        public string TradeLiecnese { get; set; }
          [DataMember]
        public string Result { get; set; }
          [DataMember]
        public string Stage { get; set; }
          [DataMember]
        public string Charges { get; set; }
         
          [DataMember]
        public string TestDate { get; set; }
        [DataMember]
          public string BankTransactionID { get; set; }

         [DataMember]
        public string ServiceType { get; set; }
         [DataMember]
        public string BarCodeString { get; set; }
         [DataMember]
         public string ConsumerCode { get; set; }



    }
}
