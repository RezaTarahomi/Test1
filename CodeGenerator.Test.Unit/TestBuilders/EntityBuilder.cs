using CodeGenerator.Core;
using CodeGenerator.Core.Dtos;
using System;

namespace CodeGenerator.Test.Unit.TestBuilders
{
    internal class EntityBuilder
    {
        private EntityCreateModel _entity { get; set; }
        public EntityBuilder()
        {
            _entity = new EntityCreateModel();
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
                Type = type
            });
            return this;
        }
    }
}
