using Entities.DataTransfertObjects.CategoryDtos;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface ICategoryService
    {
        Task<(IEnumerable<CategoryDto> categoryDtos, MetaData metaData)> GetAllCategoriesAsync(CategoryParameters categoryParameters, bool trackChanges);
        Task<CategoryDto?> GetOneCategoryByIdAsync(int id, bool trackChanges);
        Task<CategoryDto> CreateOneBookAsync(CategoryForInsertionDto categoryDto);
        Task UpdateOneCategoryAsync(int id, CategoryForUpdateDto categoryDto, bool trackChanges);
        Task DeleteOneCategoryAsync(int id, bool trackChanges);
    }
}
