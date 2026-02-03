using BuildingBlocks.Domain.Abstractions;

namespace BuildingBlocks.Domain;

public abstract class DomainService
{
    protected void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
            throw new BusinessRuleViolationException(rule);
    }
}