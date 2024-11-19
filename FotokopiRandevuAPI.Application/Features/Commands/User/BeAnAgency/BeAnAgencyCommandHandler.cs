using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.BeAnAgency
{
    public class BeAnAgencyCommandHandler : IRequestHandler<BeAnAgencyCommandRequest, BeAnAgencyCommandResponse>
    {
        private IUserService _userService;

        public BeAnAgencyCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<BeAnAgencyCommandResponse> Handle(BeAnAgencyCommandRequest request, CancellationToken cancellationToken)
        {
            var response=await _userService.BeAnAgencyRequestAsync(new()
            {
                UserName = request.UserName,
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
                AgencyName = request.AgencyName,
                Address = request.Address,
                AgencyBio=request.AgencyBio
            });
            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded,
            };
        }
    }
}
