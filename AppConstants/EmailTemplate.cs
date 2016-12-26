namespace ScottybonsMVC.AppConstants
{
    public enum OrderStatus
    {
        OrderCreated = 2,
        OrderPaid = 3,

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
        ContactUsCustomerAcknowledgemtn = 90
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
    }

}