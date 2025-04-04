using Entities.DataTransfertObjects;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace WebApi.Utilities.Formatters
{
    public class CsOutputFormatter : TextOutputFormatter
    {
        public CsOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }


        protected override bool CanWriteType(Type? type)
        {
            var result = typeof(BookDto).IsAssignableFrom(type) || typeof(IEnumerable<BookDto>).IsAssignableFrom(type);
            return result && base.CanWriteType(type);
        }
        //Simpliciter du code: We need to write this method here, we will use it later on this class
        private static void FormatCsv(StringBuilder buffer,BookDto bookDto)
        {
            buffer.AppendLine($"{bookDto.id}, {bookDto.Title},{bookDto.Price}");
        }
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response =context.HttpContext.Response;
            var buffer = new StringBuilder();
            if(context.Object is IEnumerable<BookDto>)
            {
                foreach(var book in (IEnumerable<BookDto>)context.Object)
                {
                    FormatCsv(buffer, book);    
                }
            }
            else
            {
                FormatCsv(buffer, (BookDto)context.Object!);
            }
            await  response.WriteAsync(buffer.ToString());

        }
    }
}
