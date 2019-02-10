using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOffer.Areas.Admin.Helpers
{
    public static class MenuHelper
    {
        /// <summary>
        /// Determines whether the specified controller and action is selected.
        /// </summary>
        /// <param name="controllers"> a collection of controllers names seprated by ',' . if its null, current controller will be selected.</param>
        /// <param name="actions"> a collection of actions names seprated by ',' . if its null, current action will be selected. </param>
        /// <param name="cssClass"> a css class name that will be placed. default is 'active'</param>
        /// <returns> if its Selected returns cssClass otherwise it will return an empty string</returns>
        public static string IsSelected(this IHtmlHelper htmlHelper, string controllers=null, string actions=null, string cssClass = "active")
        {
            string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
            IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : String.Empty;
        }
    }
}
