using System.Web;
using ScottybonsMVC.Models.BaseModels;
using ScottybonsMVC.Models.ViewModels;

namespace ScottybonsMVC.Models.PageViewModels
{
    public class PlanScottyBoxPageViewModel : BasePageViewModel
    {
        public IHtmlString BodyText { get; set; }
        //public PlanScottyBoxViewModel PlanScottyBoxViewModel { get; set; }
    }
}
