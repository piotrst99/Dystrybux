using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dystrybux.Model {
    public class Order {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }
        
        [Column("Name")]
        public string Name { get; set; }

        [Column("OrderedDate")]
        public string OrderedDate { get; set; }

        // do usuniecia
        [Column("FirstDate")]
        public string FirstDate { get; set; }
        
        //do usuniecia
        [Column("SecondDate")]
        public string SecondDate { get; set; }

        [Column("CompleteDate")]
        public string CompleteDate { get; set; }

        [Column("Status")]
        public string Status { get; set; }

        [Column("TotalCost")]
        public int TotalCost { get; set; }

        [Column("DeliveryID")]
        public int DeliveryID { get; set; }

        [SQLite.Ignore]
        public virtual Delivery Delivery { get; set; }

        [SQLite.Ignore]
        public virtual IEnumerable<Product> Products { get; set; }
        
    }
}
