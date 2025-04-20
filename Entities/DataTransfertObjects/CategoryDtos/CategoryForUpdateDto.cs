using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects.CategoryDtos
{
    public record CategoryForUpdateDto(int CategoryId): CategoryManipulationDto;
    
}
