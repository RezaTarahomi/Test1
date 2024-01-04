using CodeGenerator.Core.Services.Dtos;
using Database.Data;
using Database.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core.Services
{
    public class FieldService : IFieldService
    {
        private readonly CodeGeneratorDbContext _context;

        public FieldService(CodeGeneratorDbContext context)
        {
            _context = context;
        }
        public async Task Add(AddFieldModel dto)
        {

            if (dto.IsEnum)
            {
                var dbEnumType = _context.EnumTypes.Where(x => x.Name == dto.Type).FirstOrDefault();

                if (dbEnumType == null)
                {
                    var enumType = new EnumType
                    {
                        Name = dto.Type,
                    };
                    await _context.EnumTypes.AddAsync(enumType);
                    await _context.SaveChangesAsync();
                    dto.EnumTypeId = enumType.Id;
                }
                else
                {
                    dto.EnumTypeId = dbEnumType.Id;
                }
            }

            var field = new Field
            {
                Name = dto.Name,
                Type = dto.Type,
                Description = dto.Description,
                EntityId = dto.EntityId,
                EnumTypeId = dto.EnumTypeId > 0 ? dto.EnumTypeId : null,
                IsEnum = dto.IsEnum,
            };

            await _context.Fields.AddAsync(field);
            await _context.SaveChangesAsync();

        }

        public async Task Edit(EditFieldModel dto)
        {
            var field = await _context.Fields.FindAsync(dto.Id);


            if (dto.IsEnum)
            {
                if (dto.EnumTypeId > 0)
                {
                    var enumType = await _context.EnumTypes.FindAsync(dto.EnumTypeId);

                    if (dto.Type != enumType?.Name)
                    {
                        enumType = _context.EnumTypes.Where(x => x.Name == dto.Type).FirstOrDefault();

                        if (enumType != null)
                        {
                            field.EnumTypeId = enumType.Id;
                        }
                        else
                        {
                            var newEnumType = new EnumType
                            {
                                Name = dto.Type,
                            };
                            await _context.EnumTypes.AddAsync(newEnumType);
                            await _context.SaveChangesAsync();


                            field.EnumTypeId = newEnumType.Id;
                        }
                    }
                }
                else
                {
                    var enumType = _context.EnumTypes.Where(x => x.Name == dto.Type).FirstOrDefault();

                    if (enumType != null)
                    {
                        field.EnumTypeId = enumType.Id;
                    }
                    else
                    {
                        var newEnumType = new EnumType
                        {
                            Name = dto.Type,
                        };
                        await _context.EnumTypes.AddAsync(newEnumType);
                        await _context.SaveChangesAsync();


                        field.EnumTypeId = newEnumType.Id;
                    }
                }
            }
            else
            {
                field.IsEnum = false;
                field.EnumTypeId = null;

            }


            field.Name = dto.Name;
            field.Type = dto.Type;
            field.Description = dto.Description;
            field.EntityId = dto.EntityId;
            field.IsEnum = dto.IsEnum;

            await _context.SaveChangesAsync();

        }

        public async Task AddEnumField(AddEnumFieldModel dto)
        {
            var enumField = new EnumField
            {
                Name = dto.Name,
                Description = dto.Description,
                EnumTypeId = dto.EnumTypeId,
                Value = dto.Value,

            };
            await _context.EnumFields.AddAsync(enumField);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
        }

        public async Task EditEnumField(EditEnumFieldModel dto)
        {
            var enumField = await _context.EnumFields.FindAsync(dto.Id);

            enumField.Name = dto.Name;
            enumField.Value = dto.Value;
            enumField.Description = dto.Description;

            await _context.SaveChangesAsync();

        }
    }
}
