using CodeGenerator.Core.Dtos;

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
       
        public List<FileModel> Controllers { get; internal set; }
        public List<FileModel> Views { get; internal set; }
        public List<string> Methods { get; internal set; }
    }

    
}
