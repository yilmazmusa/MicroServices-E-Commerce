﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class RabbitMQSettings
    {
        public const string Stock_OrderCreatedEventQueue = "stock-order-created-event-queue";  //Burda bir sabit tanımlıyoruz.Burda diyoruz ki bir kuyruk var bu kuyruk  OrderCreatedEvent in içerisindeki dataları/verileri taşıyo hangi servis için taşıyo Stock servisi için.Bi format oluşturduk yani.
    }
}