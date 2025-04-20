namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        //Dans cette interface IRepositoryManager nous definissons nos differentes interfaces qui pointent vers nos models 
        //Et egalement la methode Save()
        
        //Ici nous n'avons pour le moment que IBookRepository
        IBookRepository Book {  get; }
        ICategoryRepository Category { get; }
        Task SaveAsync();
    }
}
