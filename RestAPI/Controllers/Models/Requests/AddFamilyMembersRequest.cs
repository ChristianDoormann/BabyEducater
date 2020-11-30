using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestAPI.Controllers.Models.Requests
{
    public class AddFamilyMembersRequest
    {
        [Required]
        public IEnumerable<string> Names { get; set; }
    }
}