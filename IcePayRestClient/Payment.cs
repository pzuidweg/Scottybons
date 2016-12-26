using ScottybonsMVC.IcepayRestClient.Classes.Payment;

namespace ScottybonsMVC.IcepayRestClient
{
    public class Payment : ServiceBase
    {
        public Payment(int merchantID, string merchantSecret) : base(merchantID, merchantSecret) { }

        public CheckoutResponse Checkout(CheckoutRequest request)
        {
            return ScottybonsMVC.IcepayRestClient.Classes.RestClient.SendAndReceive<CheckoutRequest, CheckoutResponse>("Payment", "Checkout", request, this.MerchantID, this.MerchantSecret);
        }

        public VaultCheckoutResponse VaultCheckout(VaultCheckoutRequest request)
        {
            return ScottybonsMVC.IcepayRestClient.Classes.RestClient.SendAndReceive<VaultCheckoutRequest, VaultCheckoutResponse>("Payment", "VaultCheckout", request, this.MerchantID, this.MerchantSecret);
        }

        public AutomaticCheckoutResponse AutomaticCheckout(AutomaticCheckoutRequest request)
        {
            return ScottybonsMVC.IcepayRestClient.Classes.RestClient.SendAndReceive<AutomaticCheckoutRequest, AutomaticCheckoutResponse>("Payment", "AutomaticCheckout", request, this.MerchantID, this.MerchantSecret);
        }

        public GetPaymentResponse GetPayment(GetPaymentRequest request)
        {
            return ScottybonsMVC.IcepayRestClient.Classes.RestClient.SendAndReceive<GetPaymentRequest, GetPaymentResponse>("Payment", "GetPayment", request, this.MerchantID, this.MerchantSecret);
        }

        public GetMyPaymentMethodsResponse GetMyPaymentMethods()
        {
            return ScottybonsMVC.IcepayRestClient.Classes.RestClient.SendAndReceive<GetMyPaymentMethodsRequest, GetMyPaymentMethodsResponse>("Payment", "GetMyPaymentMethods", new GetMyPaymentMethodsRequest(), this.MerchantID, this.MerchantSecret);
        }
    }
}
