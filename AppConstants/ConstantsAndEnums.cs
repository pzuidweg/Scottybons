using System.ComponentModel;

namespace ScottybonsMVC.AppConstants
{
    public enum OrderStatus
    {
        OrderCreated = 2,
        PaymentProcessInitiated = 3,
        PaymentSucess = 4,
        PaymentFailure = 5,
        PaidWithGiftCard=7,
        PartiallyPaidWithGiftCard = 8,
        AutoCreated=9,
        PartiallyPaidWithPromoCode = 10,
        PartiallyPaidWithPromoCodeGiftCard = 12,
        PaidWithPromoCode=13,
        PaidWithPromoCodeGiftCard=14
    }

    public enum EmailReason
    {
        RegistersAndCreatesAnAccount = 10,
        SignsUpForNewsLetter = 20,
        ForgotPassword = 30,
        FillsStyleProfile = 40,
        ChangesStyleProfile = 50,
        PlacesAnOrder = 60,
        PaysForStyleAdvice = 70,
        ContactUsdAdmin = 80,
        ContactUsCustomerAcknowledgemtn = 90,
        PaymentFailure = 100,
        StyleIntake = 101,
        OrderNotificationMail = 102,
        GiftCardCustomer = 103
    };

    public enum Languages
    {
        En, Nl
    };



    public enum ProfileQuestionAnswerControlTypes
    {
        TextBox = 2,
        TextArea = 3,
        CheckBox = 4,
        RadioButton = 5,
        DropdownList = 6,
        CheckBoxList = 7,
        RadioButtonList = 8,
        None = 9,
        Button = 10,
        LinkButton = 11,
        UploadControl = 12,
        Calendar = 13,
    }

    public enum StyleProfileNavigation
    {
        Order = 1,
        EditProfile = 2
    }

    public enum StyleProfileFlag
    {
        [Description("Other")]
        Other = 'O',
        [Description("None")]
        None = 'N'
    }
}

