namespace VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;

public interface IRoleServiceCqs
{
    Task<bool> IsAdminByIdUser(string userId);
}