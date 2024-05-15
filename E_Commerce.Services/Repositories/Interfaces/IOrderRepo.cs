using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Repositories.Interfaces
{
    public interface IOrderRepo:IGenericRepo<Order>
    {
    }
}
