using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Topology
{
    public static class RoutingKeys
    {
        public const string UserRegistered = "user.registered";

        public const string UserUpdated = "user.updated";

        public const string CategoryCreated = "category.created";

        public const string CategoryDeleted = "category.deleted";

        public const string ProductCreated = "product.created";

        public const string ProductUpdated = "product.updated";

        public const string ProductDeleted = "product.deleted";

        public const string ImageCreated = "image.created";

        public const string ImageUpdated = "image.updated";

        public const string ImageDeleted = "image.deleted";

        public const string OrderCreated = "order.created";


        public static class Carts
        {
            public const string CheckOut = "cart.checkout";
        }

        public static class Orders
        {
            public const string OrderCreated = "order.created";

            public const string OrderPaid = "order.paid";
        }

        public static class Payment
        {
            public const string PaymentCreated = "payment.created";
        }
    }
}
