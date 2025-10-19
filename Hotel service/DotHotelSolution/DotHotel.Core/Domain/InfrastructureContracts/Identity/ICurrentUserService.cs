namespace DotHotel.Core.Domain.InfrastructureContracts.Identity
{
    public interface ICurrentUserService
    {
        string? UserId { get; }

        string? UserRole { get; }
    }
}
