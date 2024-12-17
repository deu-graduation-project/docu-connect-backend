using MediatR;

namespace FotokopiRandevuAPI.Application.Features.Commands.Auth.VerifyResetToken
{
    public class VerifyResetTokenCommandRequest : IRequest<VerifyResetTokenCommandResponse>
    {
        public string ResetToken { get; set; }
        public string UserId { get; set; }

    }
}