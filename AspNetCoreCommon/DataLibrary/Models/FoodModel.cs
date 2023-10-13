using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    class FoodModel
    {
        //match the properties to the fields within the food table we created earlier

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
    }
}
