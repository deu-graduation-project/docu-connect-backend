using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.AssignToRole
{
    public class AssignRolesToUserCommandHandler : IRequestHandler<AssignRolesToUserCommandRequest, AssignRolesToUserCommandResponse>
    {
        readonly IUserService _userService;

        public AssignRolesToUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AssignRolesToUserCommandResponse> Handle(AssignRolesToUserCommandRequest request, CancellationToken cancellationToken)
        {
            await _userService.AssignToRolesAsync(request.UserId, request.Roles);
            return new();
        }
    }
}
