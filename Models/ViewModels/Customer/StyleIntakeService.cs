using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScottybonsMVC.AppConstants;
using ScottybonsMVC.Models.Entities;

namespace ScottybonsMVC.Models.ViewModels.Customer
{
    public class ProfileQuestionsViewModel
    {
        
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string QuestionSubDescription { get; set; }
        public bool IsRequired { get; set; }
        public bool IsImageTobeShown { get; set; }
        public string ImagePath { get; set; }
        public int DisplayOrder { get; set; }
        public string LanguageOfQuestion { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int NextQuestionNumber { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int PreviousQuestionNumber { get; set; }
        public string ValidationMessage { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int NumberOfQuestions { get; set; }
        public IList<StyleProfileAnswer> Answers { get; set; }
        public ProfileQuestionAnswerControlTypes AnswerControlType { get; set; }
        public int QuestionNumber { get; set; }
        public int AnswerControlId { get; set; }

        public bool IsDisablePrevious { get; set; }
        public bool IsDisableNext { get; set; }

        public string Percentage { get; set; }

        public string SelectedAnswer { get; set; }

        public string Flag { get; set; }
        public string SelectedOtherAnswer { get; set; }

        public string SelectedAnswerDescription { get; set; }
        public byte[] SelectedAnswerImage { get; set; }
        //public string Flag { get; set; }
        //public string SelectedOtherAnswer { get; set; }
        //public string Flag { get; set; }
        //public string SelectedOtherAnswer { get; set; }

        public int AnswerNumber { get; set; } 

        public CustomerAnswer CustomerAnswer { get; set; }

        public IList<AnswerControlViewModel> CheckBoxAnswers { get; set; }

        public string ErrorMessage { get; set; }

        //[ValidateFile]
        //public HttpPostedFileBase file { get; set; }
    }

    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            const int MAX_CONTENT_LENGTH = 1024 * 1024 * 1; //1 MB
            var allowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf" };

            var file = value as HttpPostedFileBase;

            if (file == null)
                return false;
            else if (!allowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Please upload Your Photo of type: " + string.Join(", ", allowedFileExtensions);
                return false;
            }
            else if (file.ContentLength > MAX_CONTENT_LENGTH)
            {
                ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (MAX_CONTENT_LENGTH / 1024).ToString() + "MB";
                return false;
            }
            else
                return true;
        }
    }
}