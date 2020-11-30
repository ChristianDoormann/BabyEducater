using System;
using RestAPI.Controllers.Models.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace RestAPI.Swagger.ModelExamples
{
    public class CreateBabyResponseExample : IExamplesProvider<CreateBabyResponse>
    {
        public CreateBabyResponse GetExamples()
        {
            return new CreateBabyResponse
            {
                BabyId = Guid.NewGuid()
            };
        }
    }
}