using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dystrybux.Model {
    public class Company {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }

        [Column("CompanyName")]
        public string CompanyName { get; set; }

        [Column("Street")]
        public string Street { get; set; }

        [Column("Number")]
        public string Number { get; set; }

        [Column("LocalNumber")]
        public string LocalNumber { get; set; }

        [Column("UserID")]
        public int UserID { get; set; }
    }
}
