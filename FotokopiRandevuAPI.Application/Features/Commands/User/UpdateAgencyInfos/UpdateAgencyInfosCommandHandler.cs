using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.UpdateAgencyInfos
{
    public class UpdateAgencyInfosCommandHandler : IRequestHandler<UpdateAgencyInfosCommandRequest, UpdateAgencyInfosCommandResponse>
    {
        private IUserService _userService;
        public UpdateAgencyInfosCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<UpdateAgencyInfosCommandResponse> Handle(UpdateAgencyInfosCommandRequest request, CancellationToken cancellationToken)
        {
            var response=await _userService.UpdateAgencyInfos(name:request.Name,surname:request.Surname,agencyName:request.AgencyName,province:request.Province,district:request.District,extra:request.Extra,agencyBio:request.AgencyBio,request.ProfilePhoto);
            return new()
            {
                Succeeded=response.Succeeded,
                Message = response.Message,
            };
        }
    }
}
