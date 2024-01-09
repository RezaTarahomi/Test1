using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Application.Entities;
using Application.Framework;
using AutoMapper;
using CodeGenerator.Core;
using CodeGenerator.Core.Dtos;
using CodeGenerator.Core.Services;
using CodeGenerator.Core.Services.Dtos;
using CodeGenerator.Web.Models;
using Database.Data;
using Database.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace YourNamespace.Controllers
{
    public class EntityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository _entityRepository;
        private readonly IMapper _maper;
        private readonly CodeGeneratorDbContext _context;
        private readonly IFieldService _fieldService;

        public EntityController(IUnitOfWork unitOfWork,
            IEntityRepository entityRepository,
            IMapper maper,
            CodeGeneratorDbContext context,
            IFieldService fieldService)
        {
            this._unitOfWork = unitOfWork;
            this._entityRepository = entityRepository;
            this._maper = maper;
            this._context = context;
            _fieldService = fieldService;
        }

        // GET: Entity
        public async Task<IActionResult> Index()
        {
            return View(await _entityRepository.GetAll());
        }



        // GET: Entity/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Entity/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Path,Domain")] Entity entity)
        {
            if (ModelState.IsValid)
            {
                _entityRepository.Add(entity);
                await _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        // GET: Entity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Parents = (await _entityRepository.GetAll())
                .Select(x => new Id_Caption
                {
                    Id = x.Id,
                    Caption = x.Name
                })
                .ToList();

            var entityViewModel = (await _entityRepository.GetAll())
                .Where(x => x.Id == id)
                .Select(x => new EntityViewModel
                {
                    Domain = x.Domain,
                    Name = x.Name,
                    Id = x.Id,
                    Path = x.Path,
                    Description = x.Description,                   
                    Fields = x.Fields.Select(y => new FieldViewModel
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Type = y.Type,
                        IsParent=y.IsParent, 
                        ParentId = y.ParentId,
                        IsNullable=y.IsNullable,
                        IsOneByOne=y.IsOneByOne,

                        IsForeignKey=y.IsForeignKey,
                        ForeignKeyId = y.ForeignKeyId,
                        
                        Description = y.Description,
                        IsEnum = y.IsEnum,
                        EnumType = y.EnumType != null ? new EnumTypeViewModel
                        {
                            Id = y.EnumType.Id,
                            Description = y.EnumType.Description,
                            Name = y.EnumType.Name,
                            Type = y.EnumType.Type,
                            EnumFields = y.EnumType.EnumFields.Select(e => new EnumFieldViewModel
                            {
                                Name = e.Name,
                                Value = e.Value,
                                Description = e.Description,
                                Order = e.Order,
                                Id = e.Id
                            }).ToList()
                        } : null

                    }).OrderBy(x=>x.Name).ToList(),
                })
                .FirstOrDefault();

            if (entityViewModel == null)
            {
                return NotFound();
            }
            return View(entityViewModel);
        }

        // POST: Entity/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EntityViewModel model)
        {


            var entity = await _context.Entities.FirstOrDefaultAsync(x => x.Id == model.Id);
            try
            {
                await _context.Database.BeginTransactionAsync();               

                entity.Domain = model.Domain;
                entity.Name = model.Name;
                entity.Path = model.Path;

                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EntityExists(entity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertField(UpsertFieldViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(false);
                }

                
                await _context.Database.BeginTransactionAsync();

                if (!(model.Id > 0))
                {
                    var dto = _maper.Map<AddFieldModel>(model);

                    await _fieldService.Add(dto);                      
                }
                else
                {
                    var dto = _maper.Map<EditFieldModel>(model);
                    await _fieldService.Edit(dto);                   
                }

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {

                return Json(ex);
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertEnum(UpsertEnumFieldViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(false);
                }
                await _context.Database.BeginTransactionAsync();

                if (!(model.Id > 0))
                {
                    var dto = _maper.Map<AddEnumFieldModel>(model);

                    await _fieldService.AddEnumField(dto);
                }
                else
                {
                    var dto = _maper.Map<EditEnumFieldModel>(model);
                    await _fieldService.EditEnumField(dto);
                }

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {

                return Json(ex);
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFields(int[] ids)
        {
            if (ids.Length < 1)
            {
                return Json(false);
            }

            try
            {
                var fields = await _context.Fields.Where(x => ids.Contains(x.Id)).ToArrayAsync();

                if (fields.Any(x=>x.IsParent))
                {
                    var foreignKeyIds= fields.Select(x=>x.ForeignKeyId).ToArray();    
                    var foreignKeys = await _context.Fields.Where(x => foreignKeyIds.Contains(x.Id)).ToArrayAsync();
                    _context.RemoveRange(foreignKeys);
                    await _context.SaveChangesAsync();
                }
                

                _context.RemoveRange(fields);

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEnumFields(int[] ids)
        {
            if (ids.Length < 1)
            {
                return Json(false);
            }

            try
            {
                var enumFields = await _context.EnumFields.Where(x => ids.Contains(x.Id)).ToArrayAsync();

                _context.RemoveRange(enumFields);

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClassFile(int id)
        {
            //get Entity
            var entity = await _context.Entities.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return Json(false);
            }

            //Get Class Path
            var path = Path.Combine(ProjectStructure.Domain, ProjectStructure.EntitiesFolder);

            entity.Path = Path.Combine(DirectoryHandler.GetAppRoot(), path, entity.Name );    
            await _context.SaveChangesAsync();


            //Create EntityCreateModel

            var entityModel = new EntityCreateModel { Name = entity.Name };

            entityModel.Properties = await _context.Fields
               .Where(x => x.EntityId == entity.Id)
               .Select(x => new ClassPropertyModel
               {
                   Name = x.Name,
                   Description = x.Description,
                   Type = x.Type,
                   MemberAttributes= System.CodeDom.MemberAttributes.Public,
                   IsEnum = x.IsEnum,
                   IsParent=x.IsParent,
                   IsNullable=x.IsNullable,
                   IsOneByOne=x.IsOneByOne,
                   IsForeignKey=x.IsForeignKey,
                   ParentName=x.ParentId>0?x.Parent.Name:null,

                   EnumType= x.IsEnum? new EnumType
                   {
                       Name = x.Type.Replace("?",""),
                       EnumFields = x.EnumType.EnumFields.Select(y => new EnumField
                       {
                           Name = y.Name,
                           Value = y.Value,
                       }).ToList()
                   } :  new EnumType()
               })
               .ToListAsync();

            //GenerateEntity
            EntityGenerator.GenerateEntity(path, entityModel);

            return Json(true);
        }

        public IActionResult GetProperties(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Json(null);
            }
            var properties = new List<ClassPropertyModel>();

            var entityDirectorypath = @"E:\DotNetProjects\Test\CodeGenerator\Database\Data\Entities\";
            var entityFiles = Directory.GetFiles(entityDirectorypath);

            foreach (var entityFile in entityFiles)
            {
                if (CodeGeneratorHandler.GetClassType(entityFile) == SyntaxKind.ClassKeyword)
                {
                    properties.AddRange(CodeGeneratorHandler.GetClassProperties(entityFile));

                }
            }

            properties = properties
                .Where(x => x.Name != null)
                .Where(x => x.Name.ToLower().Contains(query.ToLower()) || (x.Description != null ? x.Description.ToLower().Contains(query.ToLower()) : false))
                .DistinctBy(x => x.Name + x.Type)
                .ToList();

            return Json(properties);
        }

        // GET: Entity/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = await _entityRepository.GetById(id.Value);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: Entity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _entityRepository.GetById(id);
            _entityRepository.Delete(entity);
            await _unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EntityExists(int id)
        {
            return await _entityRepository.IsExist(id);
        }
    }
}
