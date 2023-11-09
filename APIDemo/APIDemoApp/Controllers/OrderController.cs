using DataLibrary.Data;             //added to allow us to use IFoodDatta and IOrderData
using DataLibrary.Models;           //added to allow us to access Models that we created prior
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDemoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IFoodData _foodData;       //fields used for dependency injection
        private readonly IOrderData _orderData;

        public OrderController(IFoodData foodData, IOrderData orderData)    //Constructor that points to dependencies
        {
            _foodData = foodData;
            _orderData = orderData;
        }
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Post(OrderModel order)     //Post event that is looking to see if you have a good msg or an error msg
        {
            var food = await _foodData.GetFood();                 //get list of food menu items

            order.Total = order.Quantity * food.Where(x => x.Id == order.FoodId).First().Price;     //calc total based on qty and price of selected meal
            
            int id = await _orderData.CreateOrder(order);        // ...... are good, add the record and get the id

            return Ok(id);                                      // return that we are OK (status 200) and the id of new item

            //doesnt work -- slide 21 - proj4D
        }                                                         
    }
}
