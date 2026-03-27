using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace CleanArch.Unit;

public abstract class BaseTest
{
    protected BaseTest()
    {
        Fixture = new Fixture();
        Fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
    }

    protected Fixture Fixture { get; }
}