using CodeGenerator.Core.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core.Services
{
    public interface IFieldService
    {
        Task Add(AddFieldModel model);
        Task AddEnumField(AddEnumFieldModel dto);
        Task Edit(EditFieldModel dto);
        Task EditEnumField(EditEnumFieldModel dto);
    }
}
