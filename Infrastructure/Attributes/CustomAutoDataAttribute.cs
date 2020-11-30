using AutoFixture;
using AutoFixture.Xunit2;
using Infrastructure.Customizations;

namespace Infrastructure.Attributes
{
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute() : base(() => new Fixture()
            .Customize(new TestDataCustomization()))
        {
        }
    }
}