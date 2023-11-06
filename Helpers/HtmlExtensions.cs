using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bfws.Helpers
{
    public static class HtmlExtensions
    {
        public static string IsSelected(this IHtmlHelper html, string controllers = "", string actions = "", string cssClass = "selected")
        {
            RouteValueDictionary routeValues = html.ViewContext.RouteData.Values;
            string currentAction = routeValues["Action"].ToString();
            string currentController = routeValues["Controller"].ToString();

            if (String.IsNullOrEmpty(actions))
                actions = currentAction;

            if (String.IsNullOrEmpty(controllers))
                controllers = currentController;

            string[] acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? cssClass : String.Empty;
        }

        public static IHtmlContent GetPill(this IHtmlHelper html, string id, string label, bool selected)
        {
            var pillHtml = new HtmlContentBuilder();
            pillHtml.AppendHtml(String.Format("<a class=\"nav-link {0}\" id=\"{1}-tab\" data-toggle=\"pill\" href=\"#{1}\" role=\"tab\" aria-controls=\"{1}\" aria-selected=\"{2}\">{3}</a>", selected ? "active" : "", id, selected ? "true" : "false", label));
            return pillHtml;
        }

        public static IHtmlContent GetPre(this IHtmlContent html, string jsonString)
        {
            var preHtml = new HtmlContentBuilder();
            preHtml.AppendHtml(String.Format("<pre>{0}</pre>", jsonString));
            return preHtml;
        }
            
    }
}
