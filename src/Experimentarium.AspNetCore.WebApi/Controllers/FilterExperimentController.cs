using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Experimentarium.AspNetCore.WebApi.Controllers
{
    public class CustomActionAttribute : Attribute
    {
        public string Value { get; }
        public CustomActionAttribute(string value)
        {
            this.Value = value;
        }
    }

    public class FilterExperimentController : Controller
    {
        [HttpGet("/api/experiment/filter/action/attribute")]
        [CustomAction("custom attribute value")]
        [AttributeCheckActionFilter]
        public IActionResult EmtptyAction()
        {
            return Ok();
        }
    }

    public class AttributeCheckActionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                CustomActionAttribute customActionAttribute = (controllerActionDescriptor.MethodInfo
                    .GetCustomAttributes(inherit: true)
                    .FirstOrDefault(attribute => attribute.GetType().Equals(typeof(CustomActionAttribute)))
                    ) as CustomActionAttribute;

                if (customActionAttribute != null)
                {
                    context.Result = new OkObjectResult(customActionAttribute.Value);
                }
            }
        }
    }
}
