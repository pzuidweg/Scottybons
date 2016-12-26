using ScottybonsMVC.IcepayRestClient.Classes.Refund;

namespace ScottybonsMVC.IcepayRestClient
{
    public class Refund : ServiceBase
    {
        public Refund(int merchantID, string merchantSecret) : base(merchantID, merchantSecret) { }

        public RequestRefundResponse Checkout(RequestRefundRequest request)
        {
            return ScottybonsMVC.IcepayRestClient.Classes.RestClient.SendAndReceive<RequestRefundRequest, RequestRefundResponse>("Refund", "RequestRefund", request, this.MerchantID, this.MerchantSecret);
        }

        public CancelRefundResponse Checkout(CancelRefundRequest request)
        {
            return ScottybonsMVC.IcepayRestClient.Classes.RestClient.SendAndReceive<CancelRefundRequest, CancelRefundResponse>("Refund", "CancelRefund", request, this.MerchantID, this.MerchantSecret);
        }

        public GetPaymentRefundsResponse Checkout(GetPaymentRefundsRequest request)
        {
            return ScottybonsMVC.IcepayRestClient.Classes.RestClient.SendAndReceive<GetPaymentRefundsRequest, GetPaymentRefundsResponse>("Refund", "GetPaymentRefunds", request, this.MerchantID, this.MerchantSecret);
        }
    }
}
