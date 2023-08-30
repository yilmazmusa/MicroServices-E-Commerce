using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class RabbitMQSettings
    {
        public const string Stock_OrderCreatedEventQueue = "stock-order-created-event-queue";  //Burda bir sabit tanımlıyoruz.Burda diyoruz ki bir kuyruk var bu kuyruk  OrderCreatedEvent in içerisindeki dataları/verileri taşıyo hangi servis için taşıyo Stock servisi için.Bi format oluşturduk yani.

        public const string Payment_StockReservedEventQueue = "payment-stock-reserved-event-queue";

        public const string Order_PaymentCompletedEventQueue = "order-payment-comleted-event-queue";

        public const string Order_StockNotReservedEventQueue = "order-stock-not-reserved-event-queue";
    }
}
