using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace Entities.LinkModels
{
    public class LinkParameters{
        public BookParameters BookParameters{get;init;}
        public HttpContext HttpContext{get;init;}
    }
}