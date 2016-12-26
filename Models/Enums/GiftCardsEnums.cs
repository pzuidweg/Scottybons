using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScottybonsMVC.Models
{
    public enum GiftCardCustonerType
    {
        Member=1000,
        Guest=1001,
        Admin=1002    
    }

    public enum GiftCardOrderStatus
    {
        Created = 1,
        Completed = 2,
        Error=3,
        Open=4
    }

    public enum GiftCardOrderByTypeEnum
    {
        Admin = 1,
        Customer = 2        
    }

    public enum OrderPaymentTypeEnum
    {
        CrediCard= 1,
        Ideal = 2,
        GiftCard=3,
        PromoCode=4
    }

    public enum GiftCardRedemptionStatusEnum
    {
        Success=1,
        Failure=2,
        NA=3
    }
}