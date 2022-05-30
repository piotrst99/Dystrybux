using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dystrybux.Model {
    public class OrderDelivery {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }

        [Column("OrderID"), Unique]
        public int OrderID { get; set; }

        [Column("DeliveryID")]
        public int DeliveryID { get; set; }
    }
}
