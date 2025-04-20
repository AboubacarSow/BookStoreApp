using AutoMapper;
using Entities.DataTransfertObjects.CategoryDtos;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Models
{
    public class CategoryManager : ICategoryService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public CategoryManager(IRepositoryManager manager, IMapper mapper, ILoggerService logger)
        {
            _manager = manager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto> CreateOneBookAsync(CategoryForInsertionDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
             _manager.Category.CreateOneCategory(category);
            await _manager.SaveAsync();
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task DeleteOneCategoryAsync(int id, bool trackChanges)
        {
            var category=await GetOneCategoryByIdAndCheckExist(id, trackChanges);
            _manager.Category.DeleteOneCategory(category);
            await _manager.SaveAsync();
        }
        private async Task<Category> GetOneCategoryByIdAndCheckExist(int id, bool trackChanges)
        {
            var model = await _manager.Category.GetOneCategoryByIdAsync(id, trackChanges);

            if (model is null)
            {
                string msg = $"Category with id:{id} could not be founded";
                _logger.LogInfo(msg);
                throw new CategoryNotFoundException(id.ToString());
            }

            return model;
        }

        public async Task<(IEnumerable<CategoryDto> categoryDtos, MetaData metaData)> GetAllCategoriesAsync(CategoryParameters categoryParameters, bool trackChanges)
        {
            var pagedResult = await _manager.Category.GetAllCategoriesAsync(categoryParameters, trackChanges);
            var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(pagedResult);

            return (categoryDtos: categoryDto, metaData: pagedResult.MetaData);
        }

        public async Task<CategoryDto?> GetOneCategoryByIdAsync(int id, bool trackChanges)
        {
            var category = await GetOneCategoryByIdAndCheckExist(id, trackChanges);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateOneCategoryAsync(int id, CategoryForUpdateDto categoryDto, bool trackChanges)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await GetOneCategoryByIdAndCheckExist(id, trackChanges);
            _manager.Category.UpdateOneCategory(category);
            await _manager.SaveAsync();
        }
    }
}
