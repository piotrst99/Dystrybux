using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dystrybux.Model {
    public class Delivery {
        [PrimaryKey,AutoIncrement, Column("ID")]
        public int ID { get; set; }

        [Column("CompanyName")]
        public string CompanyName { get; set; }
    
        [Column("Street")]
        public string Street { get; set; }

        [Column("Number")]
        public string Number { get; set; }

        [Column("LocalNumber")]
        public string LocalNumber { get; set; }
        
        [Column("EarliestDate")]
        public DateTime EarliestDate { get; set; }

        [Column("LatestDate")]
        public DateTime LatestDate { get; set; }

    }
}
