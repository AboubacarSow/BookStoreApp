namespace Entities.RequestFeatures
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;

        //Auto-implemented property
        public int PageNumber { get; set; }
        //Full Property


        public int PageSize { get; set; } = maxPageSize;
       
        public String? OrderBy { get; set; }
        public String? SearchTerm { get; set; }
      
    }
}
