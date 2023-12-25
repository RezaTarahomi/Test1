﻿namespace CodeGenerator.Web.Models
{
    public class FieldViewModel
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public  EnumTypeViewModel? EnumType { get; set; }
    }
}