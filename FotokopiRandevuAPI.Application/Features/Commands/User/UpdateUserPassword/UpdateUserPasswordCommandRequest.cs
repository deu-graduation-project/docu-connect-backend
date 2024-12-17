using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.UpdateUserPassword
{
    public class UpdateUserPasswordCommandRequest : IRequest<UpdateUserPasswordCommandResponse>
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string PasswordConfirm { get; set; }


    }
}
