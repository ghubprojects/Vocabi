using BuildingBlocks.Domain.Abstractions;

namespace BuildingBlocks.Domain;

public sealed class BusinessRuleViolationException(IBusinessRule brokenRule) : Exception(brokenRule.Message)
{
    public IBusinessRule BrokenRule { get; } = brokenRule;
}