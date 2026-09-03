using AutoFixture;
using NSubstitute;

namespace SampleApp.Unit;

public static class TestHelpers
{
    public static T FreezeSubstitute<T>(this Fixture fixture) where T : class
    {
        var substitute = Substitute.For<T>();
        fixture.Register(() => substitute);
        return substitute;
    }
}