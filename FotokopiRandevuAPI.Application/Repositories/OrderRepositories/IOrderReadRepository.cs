﻿using DenemeTakipAPI.Application.Repositories;
using FotokopiRandevuAPI.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Repositories.OrderRepositories
{
    public interface IOrderReadRepository: IReadRepository<Order>
    {
    }
}
