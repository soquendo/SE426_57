using Microsoft.AspNetCore.Mvc;             //added this for BadRequestObjectResults object
using Microsoft.AspNetCore.Mvc.Filters;     //added this for ActionFilterAttribute
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDemoApp.Controllers
{
    //attributes are used to decorate/add to our class similar to Data Annotations
    public class ValidateModelAttribute : ActionFilterAttribute //ValidateModel is the class and this is an Attribute so we end name with "Attribute"
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);     //was already here

            if (context.ModelState.IsValid == false)        //if model is not valid
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

    }
}
