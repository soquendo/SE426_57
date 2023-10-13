using System;
using System.Collections.Generic;
using System.ComponentModel;        //added
using System.ComponentModel.DataAnnotations;        //added
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    //this model class represents a record in the order DB table
    public class OrderModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage ="You need to keep the name to a max of 20 characters.")]
        [MinLength(3, ErrorMessage ="You need at least 3 characters for an order name.")]
        [DisplayName("Name for the Order.")]
        public string OrderName { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow; //can always be seen in the current time zone

        [DisplayName("Meal")]
        [Range(1, int.MaxValue, ErrorMessage ="You need to select a meal from the list.")]
        public int FoodId { get; set; }


        [Required]
        [Range(1, 100, ErrorMessage ="You can select up to 100 meals.")]
        public int Quantity { get; set; }

        public double Total { get; set; }

    }
}
