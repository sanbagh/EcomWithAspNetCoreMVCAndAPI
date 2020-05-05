using System;
using System.Linq.Expressions;
using Core.OrderAggregate;

namespace Core.Specifications
{
    public class OrderWithOrderItemAndOrderingSpec : BaseSpecification<Order>
    {
        public OrderWithOrderItemAndOrderingSpec(string email) : base(x => x.BuyerEmail == email)
        {
            AddIncludes(x => x.OrderedItems);
            AddIncludes(x => x.DeliveryMethod);
            AddOrderByDesc(x => x.OrderDate);
        }

        public OrderWithOrderItemAndOrderingSpec(int id, string email) : base(x => x.Id == id && x.BuyerEmail == email)
        {
            AddIncludes(x => x.OrderedItems);
            AddIncludes(x => x.DeliveryMethod);
        }
    }
}