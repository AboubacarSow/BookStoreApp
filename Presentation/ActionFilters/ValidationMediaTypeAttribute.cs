using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
public class ValidationMediaTypeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var acceptHeaderExist= context.HttpContext
        .Request
        .Headers
        .ContainsKey("Accept");
        if(!acceptHeaderExist){
            context.Result= new BadRequestObjectResult("The header 'Accept' is missing!");
            return;
        }
        var mediaType= context.HttpContext
        .Request
        .Headers["Accept"]// Headers.Accept
        .FirstOrDefault();
        if(!MediaTypeHeaderValue.TryParse(mediaType,out MediaTypeHeaderValue? outPutMediaType))
        {
            context.Result=new BadRequestObjectResult("Media Type is missing! Please enter add Accept Header with the required media type");
            return ;
        }
        context.HttpContext.Items.Add("AcceptHeaderMediaType",outPutMediaType);
    }
}