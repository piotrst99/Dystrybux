using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dystrybux.Model {
    public class OrderProduct {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }
        
        [Column("OrderID")]
        public int OrderID { get; set; }
        
        [Column("ProductID")]
        public int ProductID { get; set; }

        [Column("CountOfProducts")]
        public int CountOfProducts { get; set; }

        [SQLite.Ignore]
        public virtual Order Order { get; set; }
        [SQLite.Ignore]
        public virtual Product Product { get; set; }
    }
}
