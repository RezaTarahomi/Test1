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
                    ParentId = x.EntityParents != null ? x.EntityParents.Where(x => x.EntityId == x.Id).Select(x => x.ParentId).FirstOrDefault() : null,
                    IsOneToOne = x.EntityParents != null ? x.EntityParents.Where(x => x.EntityId == x.Id).Select(x => x.OneToOne).FirstOrDefault() : false,
                    Fields = x.Fields.Select(x => new FieldViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = x.Type,
                        Description = x.Description,
                        IsEnum = x.IsEnum,
                        EnumType = x.EnumType != null ? new EnumTypeViewModel
                        {
                            Id = x.EnumType.Id,
                            Description = x.EnumType.Description,
                            Name = x.EnumType.Name,
                            Type = x.EnumType.Type,
                            EnumFields = x.EnumType.EnumFields.Select(x => new EnumFieldViewModel
                            {
                                Name = x.Name,
                                Value = x.Value,
                                Description = x.Description,
                                Order = x.Order,
                                Id = x.Id
                            }).ToList()
                        } : null

                    }).ToList(),
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


            var entity = _maper.Map<Entity>(model);


            try
            {
                await _context.Database.BeginTransactionAsync();               
                

                var parent = await _context.EntityParents.Where(x => x.EntityId == model.Id && x.ParentId == model.ParentId).FirstOrDefaultAsync();
                if (model.ParentId > 0 && parent == null)
                {
                    var entityParent = new EntityParent
                    {
                        ParentId = model.ParentId.Value,
                        Entity = entity,
                        OneToOne = model.IsOneToOne

                    };
                    entity.EntityParents.Add(entityParent);
                }
                else if(model.ParentId > 0 && parent != null)
                {
                    parent.ParentId = model.ParentId.Value;
                    parent.OneToOne = model.IsOneToOne;
                }
                else if (model.ParentId ==null && parent != null)
                {
                   _context.RemoveRange(parent);
                }               

                _context.Update(entity);
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

            return RedirectToAction(nameof(Index));

            return View(entity);
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

                if (!(model.Id>0))
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
            if (ids.Length<1)
            {
                return Json(false);
            }

            try
            {
                var fields = await _context.Fields.Where(x => ids.Contains(x.Id)).ToArrayAsync();

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
