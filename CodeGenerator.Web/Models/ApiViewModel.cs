using CodeGenerator.Core.Dtos;
using Database.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace CodeGenerator.Web.Models
{
    public class ApiViewModel
    {
        public ApiViewModel()
        {
            Controllers = new List<FileModel>();
            Views = new List<FileModel>();
            Methods = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Controller { get; set; }
        public string View { get; set; }
        public string Method { get; set; }
        public Domains Domains { get; set; }
        public BaseType? BaseType { get; set; }
       
        public List<FileModel> Controllers { get; internal set; }
        public List<FileModel> Views { get; internal set; }
        public List<string> Methods { get; internal set; }
    }


    public class CreateApiViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ApiType ApiType { get; set; }
        public string? Url { get; set; }

        [Required]
        public Domains Domains { get; set; } 
    }

    public class RequestParameterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public bool Filterable { get; set; }
        public bool Sortable { get; set; }

        public AttributeType? AttributeType { get; set; }

        public int ApiId { get; set; }
        

    }


}
