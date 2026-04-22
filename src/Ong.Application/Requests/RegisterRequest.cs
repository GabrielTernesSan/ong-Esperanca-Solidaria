using MediatR;
using Ong.Commom;
using Ong.Domain.Enums;

namespace Ong.Application.Requests
{
    public class RegisterRequest : IRequest<Response>
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public ERole Role { get; set; } = ERole.Doador;
    }
}
