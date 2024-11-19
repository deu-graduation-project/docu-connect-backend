using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.BeAnAgencyConfirm
{
    public class BeAnAgencyConfirmCommandHandler : IRequestHandler<BeAnAgencyConfirmCommandRequest, BeAnAgencyConfirmCommandResponse>
    {
        readonly IUserService _userService;

        public BeAnAgencyConfirmCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<BeAnAgencyConfirmCommandResponse> Handle(BeAnAgencyConfirmCommandRequest request, CancellationToken cancellationToken)
        {
            var response=await _userService.BeAnAgencyConfirmAsync(new()
            {
                BeAnAgencyRequestId=request.BeAnAgencyRequestId,
                IsConfirmed=request.IsConfirmed,
            });
            return new()
            {
                Succeeded = response.Succeeded,
                Message = response.Message,
            };
        }
    }
}
