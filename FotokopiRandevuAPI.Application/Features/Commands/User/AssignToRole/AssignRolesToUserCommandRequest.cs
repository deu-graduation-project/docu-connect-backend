using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.AssignToRole
{
    public class AssignRolesToUserCommandRequest:IRequest<AssignRolesToUserCommandResponse>
    {
        public string UserId { get; set; }
        public string[]? Roles { get; set; }
    }
}
