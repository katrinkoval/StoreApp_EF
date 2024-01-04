using System.Collections.Generic;
using System.Linq;
using ModelsDTO;


namespace StoreService
{
    public static class ExtensionUtility
    {
        public static IEnumerable<Order> GetOrdersWithDiscount(this IEnumerable<Order> orders
                                                                                , double percentage)
        {
            foreach (Order o in orders)
            {
                Order orderCopy = new Order(o);
                orderCopy.TotalPrice -= (orderCopy.TotalPrice * percentage / 100.0);

                yield return orderCopy;
            }
        }

        public static IEnumerable<Order> GetOrdersWithIncrease(this IEnumerable<Order> orders
                                                                           , double percentage)
        {
            return orders.Select(o => GetOrderWithDiscount(o, percentage)); ;
        }

        //query
        //public static IEnumerable<Order> GetOrdersWithIncreaseQuery(this IEnumerable<Order> orders
        //                                                                              , double percentage)
        //{
        //    var updatedOrders = from order in orders
        //                        select GetOrderWithDiscount(order, percentage);

        //    return updatedOrders;
        //}

        public static Order GetOrderWithDiscount(this Order order, double percentage)
        {
            order.TotalPrice = order.TotalPrice - (order.TotalPrice * percentage / 100.0);

            return order;
        }

    }
}
