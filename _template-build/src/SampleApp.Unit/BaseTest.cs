using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace SampleApp.Unit;

public abstract class BaseTest
{
    protected BaseTest()
    {
        Fixture = new Fixture();
        Fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
    }

    protected Fixture Fixture { get; }
}