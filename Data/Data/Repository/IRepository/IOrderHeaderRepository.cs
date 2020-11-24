using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Data.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader OrderHeader);

    }
}
