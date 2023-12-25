namespace CodeGenerator.Web.Models
{
    public class EnumTypeViewModel
    {
        public EnumTypeViewModel()
        {
            EnumFields = new List<EnumFieldViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public List<EnumFieldViewModel> EnumFields { get; set; }
    }

    public class EnumFieldViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Value { get; set; }
        public int? Order { get; set; }
        public string? Description { get; set; }        
    }
}
