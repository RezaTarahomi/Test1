using System.ComponentModel.DataAnnotations;

namespace CodeGenerator.Web.Models
{
    public class FieldViewModel
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }
        public bool IsEnum{ get; set; }
        public bool IsParent{ get; set; }

       
        public bool IsOneByOne { get; set; }
        public int? ParentId { get; set; }        
        public bool IsNullable { get; set; }        


        public bool IsForeignKey { get; set; }
        public int? ForeignKeyId { get; set; }
        

        public  EnumTypeViewModel? EnumType { get; set; }
    }

    public class UpsertFieldViewModel
    {        
        public int? Id { get; set; }
        [Required]
        public int EntityId { get; set; }
        public int? EnumTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        public string? Description { get; set; }
        public bool IsEnum { get; set; }
        public bool IsParent { get; set; }

        public bool IsForeignKey { get; set; }
        public bool IsOneByOne { get; set; }

        public int? ParentId { get; set; }
        public bool IsNullable { get; set; }


    }
}
