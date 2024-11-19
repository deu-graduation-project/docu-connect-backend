using DenemeTakipAPI.Application.DTOs.User;
using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            if (!request.Password.Equals(request.PasswordConfirm))
                throw new Exception("Şifre ve şifre tekrar alanları uyuşmamaktadır.");

            CreateUserResponse response = await _userService.CreateUserAsync(new()
            {
                Email = request.Email,
                UserName = request.UserName,
                Name=request.Name,
                Surname=request.Surname,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
            });
            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded,
            };
        }
    }
}
