using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dystrybux.Model {
    public class User {

        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }

        [Column("Login")]
        public string Login { get; set; }
        
        [Column("Password")]
        public string Password { get; set; }
        
        [Column("Email")]
        public string Email { get; set; }
        
        [Column("Name")]
        public string Name { get; set; }
        
        [Column("Surname")]
        public string Surname { get; set; }

        [Column("Role")]
        public string Role { get; set; }


    }
}
