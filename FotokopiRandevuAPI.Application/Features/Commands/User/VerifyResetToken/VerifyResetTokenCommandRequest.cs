﻿using FotokopiRandevuAPI.Application.Features.Commands.User.VerifyResetToken;
using MediatR;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.VerifyResetToken
{
    public class VerifyResetTokenCommandRequest:IRequest<VerifyResetTokenCommandResponse>
    {
        public string ResetToken { get; set; }
        public string UserId { get; set; }

    }
}