using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RestAPI.Swagger.ModelExamples;
using Swashbuckle.AspNetCore.Filters;

namespace RestAPI.Swagger.Extensions
{
    public static class ServiceCollectionSwaggerExtensions
    {
        public static IServiceCollection SetupSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo 
                {
                    Title = "Baby Educator", 
                    Version = "V1",
                    Description = API_DESCRIPTION
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, true);
                options.ExampleFilters();
            });
            services.AddSwaggerExamplesFromAssemblyOf(typeof(AddWordsToVocabularyRequestExample));
            
            return services;
        }
        
        private const string API_DESCRIPTION = @"## Baby Educator API v1
The API for the concious parent who do not want to slack on their Baby's education in language.

**Introduction**:
With Google Home taped to your baby, you can now surveil your new-born baby 24/7. 
Google Home can hook up to this API and provide the recordings of your kid's speech 
and keep track of all the distinct words spoken through the babys lifetime as well as
note when the kid can say the name of known family members.

The API works with three concepts:
* **Baby**: A Baby with a vocabulary and a family. You can extend the vocabulary and in return see which words were newly learned.
* **Vocabulary**: A distinct set of words learned by the baby. It can be extended whenever Google Home registers its latest recordings.
* **Family list**: A list of family member's names. Each time the vocabulary is extended, the api responds if the baby learned some new family member's names.

How to use it:
* Create your baby.
* Add known family members.
* Hook up with Google Home (Service not implemented yet), and have it register your baby's spoken words in appropriate intervals.";
    }
}