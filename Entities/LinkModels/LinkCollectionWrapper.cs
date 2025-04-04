namespace Entities.LinkModels
{
    
    public class LinkCollectionWrapper<T> : LinkRessourceBase
    {
        public List<T> Value { get; set; } = new List<T>();

        public LinkCollectionWrapper(List<T> value)
        {
            Value = value;
        }
        public LinkCollectionWrapper()
        {

        }
    }
}