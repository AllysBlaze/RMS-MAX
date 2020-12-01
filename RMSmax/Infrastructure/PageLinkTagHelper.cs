using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RMSmax.Models.ViewModels;

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

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div");
            if (PageModel.TotalPages > 5) //Zmniejszenie liczby wyswietlanych numerow stron
            {
                TagBuilder tag = new TagBuilder("a");
                if (PageModel.CurrentPage > 2)
                {
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = 1 });
                    tag.InnerHtml.Append(1.ToString());
                    result.InnerHtml.AppendHtml(tag);
                }
                for (int i = PageModel.CurrentPage - 1; i <= PageModel.CurrentPage + 1; i++)
                {
                    if (i > 0 && i <= PageModel.TotalPages)
                    {
                        tag = new TagBuilder("a");
                        tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i });
                        tag.InnerHtml.Append(i.ToString());
                        result.InnerHtml.AppendHtml(tag);
                    }
                }
                if (PageModel.CurrentPage < PageModel.TotalPages - 1)
                {
                    tag = new TagBuilder("a");
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = PageModel.TotalPages });
                    tag.InnerHtml.Append(PageModel.TotalPages.ToString());
                    result.InnerHtml.AppendHtml(tag);
                }
            }
            else //Wszystkie numery stron
            {
                for (int i = 1; i <= PageModel.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i });
                    tag.InnerHtml.Append(i.ToString());
                    result.InnerHtml.AppendHtml(tag);
                }
            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}