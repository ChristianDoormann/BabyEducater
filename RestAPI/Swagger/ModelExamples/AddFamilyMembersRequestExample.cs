using RestAPI.Controllers.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace RestAPI.Swagger.ModelExamples
{
    public class AddFamilyMembersRequestExample : IExamplesProvider<AddFamilyMembersRequest>
    {
        public AddFamilyMembersRequest GetExamples()
        {
            return new AddFamilyMembersRequest
            {
                Names = new[] {"Papa", "Granny", "Kaj"}
            };
        }
    }
}