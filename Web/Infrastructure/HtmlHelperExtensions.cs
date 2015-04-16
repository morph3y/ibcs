using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Web.Infrastructure
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString FieldFor<TModel, TType>(this HtmlHelper helper, Expression<Func<TModel, TType>> @for) where TModel : class
        {
            var expression = (MemberExpression)@for.Body;
            string name = expression.Member.Name;
            var modelInstance = helper.ViewData.Model as TModel;
            if (modelInstance != null)
            {
                helper.ViewContext.Writer.Write("<div class=\"field\">");
                {
                    helper.ViewContext.Writer.Write(helper.Label(name));
                    helper.ViewContext.Writer.Write("<div class=\"value\">");
                    helper.ViewContext.Writer.Write(helper.Editor(name));
                    helper.ViewContext.Writer.Write("</div>");
                }
                helper.ViewContext.Writer.Write("</div>");
            }
            return null;
        }
    }
}