using CodeGenerator.Core.Dtos;
using System.Collections.Generic;

namespace CodeGenerator.Test.Unit.TestBuilders
{
    internal class MethodSignBuilder
    {
        public MethodSignModel _method { get; set; }
        public MethodSignBuilder()
        {
            _method = new MethodSignModel();
            _method.Parameters = new List<ClassPropertyModel>();
        }

        public MethodSignBuilder WithName(string name)
        {
            _method.Name = name;
            return this;
        }

        public MethodSignBuilder WithResponseType(string responseType)
        {
            _method.ResponseType = responseType;
            return this;
        }

        public MethodSignBuilder WithResponseVariableName(string variableName)
        {
            _method.ResponseVariableName = variableName;
            return this;
        }

        public MethodSignBuilder WithParameters(string name, string type)
        {
            _method.Parameters.Add(new ClassPropertyModel
            {
                Name = name,
                Type = type
            });
            return this;
        }

        public MethodSignModel Build()
        {
            return _method;
        }
    }
}
