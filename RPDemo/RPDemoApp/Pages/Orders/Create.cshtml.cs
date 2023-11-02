using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;             //Added
using DataLibrary.Models;           //Added
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;       //Added

namespace RPDemoApp.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        public List<SelectListItem> FoodItems { set; get; }     //List of menu items to fill in drop down list of menu options

        [BindProperty]                                          //This states this can be filled (written to) by a POST from the FORM in the VIEW
        public OrderModel Order { get; set; }                   //Object that will be filled with an order of one type of menu item

        public CreateModel(IFoodData foodData, IOrderData orderData)    //constructor that receives objects that facilitate Food and Order Data transactions
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        public async Task OnGet()
        {
            var food = await _foodData.GetFood();       //create container (food) and fill it with the menu items using our built-n function from DLL

            FoodItems = new List<SelectListItem>();     //making sure the list of food menu items is clean at the start

            //loop thru the list, where "x" represents the current food menu item on the current iteration
            food.ForEach(x =>
            {
                FoodItems.Add(new SelectListItem { Value = x.Id.ToString(), Text = x.Title });  //Add each item to the local list storing Id value but showing title
            }
            );
        }

        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid == false) //how else could i write this? why write it this way
            {
                return Page();
            }
            else
            {
                //*************
                //calculate total (qty * price)

                var food = await _foodData.GetFood();       //get list of food items

                Order.Total = Order.Quantity * food.Where(x => x.Id == Order.FoodId).First().Price;     //get price of mathcing item and calc total cost
                                                                                                        // can this be done more efficiently
                                                                                                        
                //******************
                // perform the insertion into the DB table
                // redirect to a new copy of the ordering page

                int id = await _orderData.CreateOrder(Order);     //insert the record using the DB libraries we created and return the ID of new record
                return RedirectToPage("./Create");              //right now we just return to a fresh copy of this page
            }
        }

    }
}
