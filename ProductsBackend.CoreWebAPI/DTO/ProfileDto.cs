using System.Collections.Generic;

namespace ProductsBackend.CoreWebAPI.DTO
{
    public class ProfileDto
    {
        public List<string> Permissions { get; set; }
        public string Name { get; set; }
    }
}