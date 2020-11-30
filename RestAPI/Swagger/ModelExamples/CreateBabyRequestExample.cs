using RestAPI.Controllers.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace RestAPI.Swagger.ModelExamples
{
    public class CreateBabyRequestExample : IExamplesProvider<CreateBabyRequest>
    {
        public CreateBabyRequest GetExamples()
        {
            return new CreateBabyRequest
            {
                Name = "Roger Rabbit",
                Age = 1
            };
        }
    }
}