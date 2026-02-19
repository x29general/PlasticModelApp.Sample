using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using PlasticModelApp.Application.Shared;
using Xunit;
using AppValidationException = PlasticModelApp.Application.Shared.Exceptions.ValidationException;

namespace PlasticModelApp.Application.UnitTests.Shared;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_Invokes_Next_When_No_Validators()
    {
        var sut = new ValidationBehavior<DummyRequest, string>(
            Enumerable.Empty<IValidator<DummyRequest>>(),
            NullLogger<ValidationBehavior<DummyRequest, string>>.Instance);
        var nextCalled = false;

        var result = await sut.Handle(
            new DummyRequest("ok"),
            _ =>
            {
                nextCalled = true;
                return Task.FromResult("done");
            },
            CancellationToken.None);

        nextCalled.Should().BeTrue();
        result.Should().Be("done");
    }

    [Fact]
    public async Task Handle_Invokes_Next_When_Validation_Passes()
    {
        var validators = new IValidator<DummyRequest>[] { new DummyRequestValidator() };
        var sut = new ValidationBehavior<DummyRequest, string>(
            validators,
            NullLogger<ValidationBehavior<DummyRequest, string>>.Instance);
        var nextCalled = false;

        var result = await sut.Handle(
            new DummyRequest("ok"),
            _ =>
            {
                nextCalled = true;
                return Task.FromResult("done");
            },
            CancellationToken.None);

        nextCalled.Should().BeTrue();
        result.Should().Be("done");
    }

    [Fact]
    public async Task Handle_Throws_ApplicationValidationException_When_Validation_Fails()
    {
        var validators = new IValidator<DummyRequest>[] { new DummyRequestValidator() };
        var sut = new ValidationBehavior<DummyRequest, string>(
            validators,
            NullLogger<ValidationBehavior<DummyRequest, string>>.Instance);
        var nextCalled = false;

        var ex = await Assert.ThrowsAsync<AppValidationException>(() =>
            sut.Handle(
                new DummyRequest(string.Empty),
                _ =>
                {
                    nextCalled = true;
                    return Task.FromResult("done");
                },
                CancellationToken.None));

        nextCalled.Should().BeFalse();
        ex.Code.Should().Be("E-400-001");
        ex.Details.Should().NotBeEmpty();
    }

    private sealed record DummyRequest(string Name) : IRequest<string>;

    private sealed class DummyRequestValidator : AbstractValidator<DummyRequest>
    {
        public DummyRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
