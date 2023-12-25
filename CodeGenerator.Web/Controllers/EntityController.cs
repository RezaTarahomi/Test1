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

        public EntityController(IUnitOfWork unitOfWork,IEntityRepository entityRepository
            ,IMapper maper, CodeGeneratorDbContext context)
        {
            this._unitOfWork = unitOfWork;
            this._entityRepository = entityRepository;
            this._maper = maper;
            this._context = context;
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

            var entityViewModel = (await _entityRepository.GetAll())
                .Where(x => x.Id == id)
                .Select(x => new EntityViewModel
                {
                    Domain = x.Domain,
                    Name = x.Name,
                    Id = x.Id,
                    Path = x.Path,
                    Fields = x.Fields.Select(x => new FieldViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = x.Type,
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
                        }:null

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

             var entity= _maper.Map<Entity>(model);           
           
                try
                {
               await _context.Database.BeginTransactionAsync();

                var fields =await _context.Fields.Where(x => x.EntityId == model.Id).ToListAsync();
                _context.Fields.RemoveRange(fields);

                foreach (var item in entity.Fields)
                {
                    item.EntityId = model.Id;
                }
                await _context.AddRangeAsync(entity.Fields.ToList());

                _context.Update(entity);
                   await  _context.SaveChangesAsync();

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
                }catch (Exception ex)
            {
                throw;
            }

                return RedirectToAction(nameof(Index));
            
            return View(entity);
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

            properties= properties
                .Where(x => x.Name != null)
                .Where(x=>x.Name.ToLower().Contains(query.ToLower()))
                .DistinctBy(x=>x.Name+x.Type)
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
