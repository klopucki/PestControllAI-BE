using System.Net;
using FluentAssertions;
using OrdersSomething.Command.Api.Features.Properties.Commands;
using OrdersSomething.Query.Api.Features.Properties;

namespace OrdersSomething.Tests.Properties;

internal class PropertiesAssertions
{
    internal static void AssertProperties(UpsertPropertyCommand expected, PropertiesDto? actual)
    {
        actual.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.Id));
    }

    internal static void AssertProperties(DeletePropertyCommand expected, PropertiesDto? actual)
    {
        actual.IsDeleted.Should().Be(expected.IsDeleted);
    }

    public static void AssertNotFound(HttpResponseMessage response)
    {
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}