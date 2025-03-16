namespace Entities.RequestFeatures
{
    public class BookParameters : RequestParameters
    {
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; } = 1000;
        public bool ValidePriceRange => MaxPrice > MinPrice;
        public String? SearchTerm { get; set; }
        public String? OrderBy { get; set; }
    }
}
