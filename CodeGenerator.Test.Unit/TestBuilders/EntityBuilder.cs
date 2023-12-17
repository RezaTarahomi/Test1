using CodeGenerator.Core;
using CodeGenerator.Core.Dtos;
using System;
using System.Collections.Generic;

namespace CodeGenerator.Test.Unit.TestBuilders
{
    internal class EntityBuilder
    {
        private EntityCreateModel _entity { get; set; }
        public EntityBuilder()
        {
            _entity = new EntityCreateModel();
            _entity.Properties = new List<ClassPropertyModel>();
        }


        public EntityBuilder WithName(string name)
        {
            _entity.Name = name;
            return this;
        }

        public EntityCreateModel Build()
        {
            return _entity;
        }

        public EntityBuilder WithProperty(string name, Type type)
        {
            _entity.Properties.Add(new ClassPropertyModel
            {
                Name = name,
                Type = type.ToString()
            });
            return this;
        }

        public EntityBuilder WithProperty(string name, string type)
        {
            _entity.Properties.Add(new ClassPropertyModel
            {
                Name = name,
                Type = type
            });
            return this;
        }
    }
}
