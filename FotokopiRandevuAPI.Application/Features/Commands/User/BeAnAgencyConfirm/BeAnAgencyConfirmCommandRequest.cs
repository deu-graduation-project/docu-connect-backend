using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.BeAnAgencyConfirm
{
    public class BeAnAgencyConfirmCommandRequest:IRequest<BeAnAgencyConfirmCommandResponse>
    {
        public string BeAnAgencyRequestId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
