using System;
using RestAPI.Controllers.Models.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace RestAPI.Swagger.ModelExamples
{
    public class GetBabyResponseExample : IExamplesProvider<GetBabyResponse>
    {
        public GetBabyResponse GetExamples()
        {
            return new GetBabyResponse
            {
                BabyId = Guid.NewGuid(),
                Name = "Roger Rabbit",
                Age = 1,
                Vocabulary = new [] {"My", "only", "purpose", "in", "life", "is", "to", "make", "people", "laugh"},
                FamilyMembers = new [] {"Eddie", "Jessica"}
            };
        }
    }
}