﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.Product.DeleteAgencyProducts
{
    public class DeleteAgencyProductsCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
