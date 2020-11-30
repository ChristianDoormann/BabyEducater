using System.ComponentModel.DataAnnotations;

namespace RestAPI.Controllers.Models.Requests
{
    public class CreateBabyRequest
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public int Age { get; set; }
    }
}