using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RMSmax.Models.ViewModels;
using System.Collections.Generic;

namespace RMSmax.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-argument-")]
        public Dictionary<string, object> PageArgumentsValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder result = new TagBuilder("div");

            if (PageModel.TotalPages > 1)
            {
                if (PageModel.TotalPages > 5)
                {
                    if (PageModel.CurrentPage >= 1 && PageModel.CurrentPage <= 3)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            result.InnerHtml.AppendHtml(CreatePageBtnTag(i));
                        }
                        result.InnerHtml.AppendHtml(CreateDotsBtnTag());
                        result.InnerHtml.AppendHtml(CreatePageBtnTag(PageModel.TotalPages));
                    }
                    else if (PageModel.CurrentPage >= PageModel.TotalPages - 2 && PageModel.CurrentPage <= PageModel.TotalPages)
                    {
                        result.InnerHtml.AppendHtml(CreatePageBtnTag(1));
                        result.InnerHtml.AppendHtml(CreateDotsBtnTag());
                        for (int i = PageModel.TotalPages - 3; i <= PageModel.TotalPages; i++)
                        {
                            result.InnerHtml.AppendHtml(CreatePageBtnTag(i));
                        }
                    }
                    else
                    {
                        result.InnerHtml.AppendHtml(CreatePageBtnTag(1));
                        result.InnerHtml.AppendHtml(CreateDotsBtnTag());
                        for (int i = PageModel.CurrentPage - 1; i <= PageModel.CurrentPage + 1; i++)
                        {
                            result.InnerHtml.AppendHtml(CreatePageBtnTag(i));
                        }
                        result.InnerHtml.AppendHtml(CreateDotsBtnTag());
                        result.InnerHtml.AppendHtml(CreatePageBtnTag(PageModel.TotalPages));
                    }
                }
                else
                {
                    for (int i = 1; i <= PageModel.TotalPages; i++)
                    {
                        result.InnerHtml.AppendHtml(CreatePageBtnTag(i));
                    }
                }
                output.Content.AppendHtml(result.InnerHtml);
            }
        }

        private TagBuilder CreatePageBtnTag(int i)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder tag = new TagBuilder("a");
            PageArgumentsValues["page"] = i;
            tag.Attributes["href"] = urlHelper.Action(PageAction, PageArgumentsValues);
            tag.AddCssClass("btn");
            tag.AddCssClass(i == PageModel.CurrentPage ? "btn-primary" : "btn-secondary");
            tag.InnerHtml.Append(i.ToString());

            return tag;
        }
        private TagBuilder CreateDotsBtnTag()
        {
            TagBuilder tag = new TagBuilder("a");
            tag.AddCssClass("btn");
            tag.AddCssClass("btn-secondary");
            tag.AddCssClass("dots");
            tag.InnerHtml.Append(". . .");

            return tag;
        }
    }
}