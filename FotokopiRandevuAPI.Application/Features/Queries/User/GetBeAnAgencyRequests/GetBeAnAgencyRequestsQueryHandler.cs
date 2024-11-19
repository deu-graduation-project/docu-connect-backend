using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.User.GetBeAnAgencyRequests
{
    public class GetBeAnAgencyRequestsQueryHandler : IRequestHandler<GetBeAnAgencyRequestsQueryRequest, GetBeAnAgencyRequestsQueryResponse>
    {
        readonly IUserService _userService;
        public GetBeAnAgencyRequestsQueryHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<GetBeAnAgencyRequestsQueryResponse> Handle(GetBeAnAgencyRequestsQueryRequest request, CancellationToken cancellationToken)
        {
            var response=await _userService
                        .GetBeAnAgencyRequests(request.Page, request.Size, request.OrderBy,request.RequestId,request.UsernameOrEmail,request.State);
            return new()
            {
                TotalCount = response.TotalCount,
                BeAnAgencyRequests = response.BeAnAgencyRequests
            };
        }
    }
}
