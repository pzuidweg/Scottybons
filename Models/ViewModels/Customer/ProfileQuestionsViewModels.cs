using System.Collections.Generic;
using System.Globalization;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace ScottybonsMVC.Models.ViewModels.Customer
{
    public class ProfileQuestionsVm : RenderModel
    {
        public ProfileQuestionsVm(IPublishedContent content, CultureInfo culture) : base(content, culture) { }

        public ProfileQuestionsVm(IPublishedContent content) : base(content) { }

        public ProfileQuestionsVm() : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent) { }

        public ProfileQuestionsViewModel ProfileQuestionsViewModel { get; set; }
        public List<ProfileQuestionsViewModel> ProfileQuestionsForEditAndView { get; set; }

        public bool IsNewCustomer { get; set; }
    }

    public class AnswerControlViewModel
    {
        public bool IsSelected { get; set; }
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public int DependentQuestion { get; set; }
        public string AnswerDescription { get; set; }
        public string AnswerImagePath { get; set; }
        public string Flag { get; set; }
        public string QuestionNumber { get; set; }
        public string OtherAnswer { get; set; }
        public string AnswerNumber { get; set; }
    }
}