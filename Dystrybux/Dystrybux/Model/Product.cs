using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;

namespace Dystrybux.Model {
    public class Product {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }
        
        [Column("Name")]
        public string Name { get; set; }
        
        [Column("Count")]
        public int Count { get; set; }
        
        [Column("Cost")]
        public int Cost { get; set; }
        
        [Column("Description")]
        public string Description { get; set; }
    
        [Column("ImagePatgh")]
        public string ImagePath { get; set; }

        [SQLite.Ignore]
        public ImageSource Image { get; set; }
    }
}
    