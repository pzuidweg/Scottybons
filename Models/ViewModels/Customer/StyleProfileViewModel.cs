using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Resources.Scottybons;
using ScottybonsMVC.Models.Entities;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace ScottybonsMVC.Models.ViewModels.Customer
{
    public class StyleProfileViewModel : PostRedirectModel
    {
        public IEnumerable<StyleProfileQuestion> ProfileQuestion { get; set; }
        public IEnumerable<StyleProfileAnswer> ProfileAnswers { get; set; }
        public IEnumerable<AnswerTypeofControl> AnswerTypeControls { get; set; }
    }


    public class StyleProfileVm : RenderModel
    {
        public StyleProfileVm(IPublishedContent content, CultureInfo culture)
            : base(content, culture)
        {
        }
        public StyleProfileVm(IPublishedContent content)
            : base(content)
        {
        }
        public StyleProfileVm() : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent) { }


        public StyleProfileViewModel StyleProfileViewModel { get; set; }
    }

    public class StyleProfileModel
    {
        public string StyleProfileId { get; set; }
        public string StyleProfileType { get; set; }
        public string StyleProfileChecked { get; set; }

    }

    public class StyleProfileDisplayVm : RenderModel
    {
        public StyleProfileDisplayVm(IPublishedContent content, CultureInfo culture)
            : base(content, culture)
        {

            StyleProfileTypes = new List<StyleProfileModel>
            {
                new StyleProfileModel
                {
                    StyleProfileId = "1",
                    StyleProfileType =
                        Resources.Scottybons.ScottybonsDataStrings.YesIcanmovedirectlythrutoorderingmynextScottybox,
                    // "Yes, I can move directly thru to ordering my next scottybox",
                    StyleProfileChecked = "checked"
                },
                new StyleProfileModel
                {
                    StyleProfileId = "2",
                    StyleProfileType = Resources.Scottybons.ScottybonsDataStrings.Nomaybenot_I_willreviewitfirst,
                    //"No, maybe not, i will review it first",
                    StyleProfileChecked = string.Empty
                }
            };
        }

        public StyleProfileDisplayVm(IPublishedContent content)
            : base(content)
        {

            StyleProfileTypes = new List<StyleProfileModel>
            {
                new StyleProfileModel
                {
                    StyleProfileId = "1",
                    StyleProfileType =
                        Resources.Scottybons.ScottybonsDataStrings.YesIcanmovedirectlythrutoorderingmynextScottybox,
                    // "Yes, I can move directly thru to ordering my next scottybox",
                    StyleProfileChecked = "checked"
                },
                new StyleProfileModel
                {
                    StyleProfileId = "2",
                    StyleProfileType = Resources.Scottybons.ScottybonsDataStrings.Nomaybenot_I_willreviewitfirst,
                    //"No, maybe not, i will review it first",
                    StyleProfileChecked = string.Empty
                }
            };
        }

        public StyleProfileDisplayVm()
            : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent)
        {

            StyleProfileTypes = new List<StyleProfileModel>
            {
                new StyleProfileModel
                {
                    StyleProfileId = "1",
                    StyleProfileType =
                        Resources.Scottybons.ScottybonsDataStrings.YesIcanmovedirectlythrutoorderingmynextScottybox,
                    // "Yes, I can move directly thru to ordering my next scottybox",
                    StyleProfileChecked = "checked"
                },
                new StyleProfileModel
                {
                    StyleProfileId = "2",
                    StyleProfileType = Resources.Scottybons.ScottybonsDataStrings.Nomaybenot_I_willreviewitfirst,
                    //"No, maybe not, i will review it first",
                    StyleProfileChecked = string.Empty
                }
            };
        }

        [Display(ResourceType = typeof(ScottybonsDataStrings), Name = "OrderVm_StyleProfileHeader_Is_your_Style_Profile_Still_UptoDate")]
        public string StyleProfileHeader { get; set; }

        public List<StyleProfileModel> StyleProfileTypes { get; set; }

        public int SelectedStyleProfileId { get; set; }
    }
}