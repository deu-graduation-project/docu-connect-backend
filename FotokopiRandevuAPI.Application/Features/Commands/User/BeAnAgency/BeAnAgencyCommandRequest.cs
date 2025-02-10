using FotokopiRandevuAPI.Domain.Entities.Identity.Extra;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.BeAnAgency
{
    public class BeAnAgencyCommandRequest:IRequest<BeAnAgencyCommandResponse>
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public string AgencyName { get; set; }
        public Address Address { get; set; }
        public string? AgencyBio { get; set; }

        [JsonIgnore]
        public IFormFile? ProfilePhoto { get; set; }
    }
}
