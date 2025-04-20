using System.Reflection;
using Services.Contracts;
namespace Services.Models
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] Properties{get;set;}

        public DataShaper()
        {
            Properties = typeof(T)
                            .GetProperties(BindingFlags.Public |BindingFlags.Instance);
        }
        private List<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            var requiredFiels=new List<PropertyInfo>();
            if(!String.IsNullOrEmpty(fieldsString))
            {
                var fields= fieldsString.Split(",",StringSplitOptions.RemoveEmptyEntries);
                foreach(var field in fields)
                {
                    var property= Properties
                                    .FirstOrDefault
                                    (p=>p.Name.Equals(field,StringComparison.CurrentCultureIgnoreCase));
                    if(property is null)
                        continue;
                    requiredFiels.Add(property);
                }
            }
            else{
                requiredFiels=Properties.ToList();
            }
            return requiredFiels;
        }
        private ShapedEntity FetchDataFromEntity(T entity, IEnumerable<PropertyInfo> requiredProperty)
        {
            var shapedObject=new ShapedEntity();
            foreach(var property in requiredProperty){
                var objectPropertyValue=property.GetValue(entity);
                shapedObject?.Entity!.TryAdd(property.Name,objectPropertyValue);
            }
            var objectProperty=entity.GetType().GetProperty("id");
            shapedObject!.Id=(int)objectProperty?.GetValue(entity)!;
            return shapedObject;
        }
        private List<ShapedEntity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperty)
        {
            var shapdedEntities= new List<ShapedEntity>();
            foreach(var entity in entities){
                var shapedEntity=FetchDataFromEntity(entity,requiredProperty);
                shapdedEntities.Add(shapedEntity);
            }
            return shapdedEntities;
        }
        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            var requiredProperty= GetRequiredProperties(fieldsString);
            return FetchData(entities,requiredProperty);
        }

        public ShapedEntity ShapeData(T entity, string fieldsString)
        {
            var requiredProperty= GetRequiredProperties(fieldsString);
            return FetchDataFromEntity(entity, requiredProperty);
        }
    }
    
}