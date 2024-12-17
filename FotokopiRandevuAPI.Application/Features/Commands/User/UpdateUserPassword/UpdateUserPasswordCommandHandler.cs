using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.UpdateUserPassword
{
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommandRequest, UpdateUserPasswordCommandResponse>
    {
        readonly IUserService _userService;

        public UpdateUserPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdateUserPasswordCommandResponse> Handle(UpdateUserPasswordCommandRequest request, CancellationToken cancellationToken)
        {
            if (!request.NewPassword.Equals(request.PasswordConfirm))
                throw new Exception("Şifre ve şifre tekrar alanları uyuşmamaktadır.");
            var response = await _userService.UpdateUserPassword(request.CurrentPassword, request.NewPassword);
            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded,
            };
        }
    }
}
