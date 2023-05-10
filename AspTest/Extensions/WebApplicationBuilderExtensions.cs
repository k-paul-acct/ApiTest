using AspTest.Contracts.Requests.Models;
using FluentValidation;

namespace AspTest.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<Register>();

        return builder;
    }
}