using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Opds4Net.Web.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class HtmlExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString HiddenForWithValue<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string value, object htmlAttributes)
        {
            var propertyName = ExpressionHelper.GetExpressionText(expression);

            var input = new TagBuilder("input");
            input.MergeAttribute("id", helper.AttributeEncode(helper.ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)));
            input.MergeAttribute("name", helper.AttributeEncode(helper.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName)));
            input.MergeAttribute("value", value);
            input.MergeAttribute("type", "hidden");
            input.MergeAttributes(htmlAttributes as IDictionary<string, object>);

            return MvcHtmlString.Create(input.ToString());
        }
    }
}