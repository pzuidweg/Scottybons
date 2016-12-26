namespace ScottybonsMVC.Models.Interfaces
{
    public interface IBasket
    {
        bool AllowBasketEdit { get; }

        bool ShowOrderTotal { get; }
    }
}
