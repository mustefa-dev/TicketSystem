using TicketSystem.DATA.DTOs.User;

namespace e_parliament.Interface
{
    public interface ITokenService
    {
        string CreateToken(TokenDTO user);
    }
}