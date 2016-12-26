using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScottybonsMVC.Models.Entities;

namespace ScottybonsMVC.Models.ViewModels.Admin
{
    public class OrdersViewModel
    {
        public OrdersViewModel()
        {

        }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFName { get; set; }
        public string CustomerLName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string OrderStatus { get; set; }
        public int OrderStatusId { get; set; }

        public string Occasion { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string PostalCode { get; set; }
        public string Town { get; set; }

        public string Country { get; set; }
        public int CountryId { get; set; }
        //Permission to deliver at neighbours?
        public int PermissionToDeliverAtNeighbours { get; set; }
        public int PermissionToCall { get; set; }
        public int Subscription { get; set; }
        public int PeriodForSubscripion { get; set; }

        public IEnumerable<StyleIntake> StyleIntakeCustomerAnswers { get; set; }

    }

    public class StyleIntake
    {
        public string QuestionNumber { get; set; }
        public string Question { get; set; }
        public string SelectedAnswer { get; set; }
        public string AnswerControlType { get; set; }
        public byte[] AnswerImage { get; set; }
        public string OtherFlag { get; set; }
        public string Lang { get; set; }

    }
}