using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.User.GetSingleAgency
{
    public class GetSingleAgencyQueryHandler : IRequestHandler<GetSingleAgencyQueryRequest, GetSingleAgencyQueryResponse>
    {
        readonly IUserService _userService;

        public GetSingleAgencyQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetSingleAgencyQueryResponse> Handle(GetSingleAgencyQueryRequest request, CancellationToken cancellationToken)
        {
            var response=await _userService.GetSingleAgency(request.AgencyId);
            return new()
            {
                Agency = response.Agency,
            };
        }
    }
}
