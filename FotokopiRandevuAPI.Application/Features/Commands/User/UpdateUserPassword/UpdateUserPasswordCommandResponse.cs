using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.UpdateUserPassword
{
    public class UpdateUserPasswordCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }

    }
}
