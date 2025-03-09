using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;

namespace MyApp.Namespace
{
    public class OrderController(IHttpClientFactory httpClientFactory) : BaseController
    {
        private HttpClient Client => httpClientFactory.CreateClient("Api.Data");
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var response = await Client.DeleteAsync($"api/delete/order/{orderId}");

            if(!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to delete order");
                return RedirectToAction("NotFound", "Error");
            }

            return RedirectToAction("ListOrders");
        }
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var userId = GetUserId();
            if(userId is null)
            {
                SetErrorMessage("You must login");
                return RedirectToAction("NotFound", "Error");
            }
            if (orderId <= 0)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var response = await Client.GetAsync($"/api/orders/{orderId}");

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to load order");
                return RedirectToAction("NotFound", "Error");
            }

            var order = await response.Content.ReadFromJsonAsync<OrderEntity>();

            if (order == null)
            {
                ViewBag.Error = "Order not found";
                return RedirectToAction("NotFound", "Error");
            }
            if(userId != order.UserId)
            {
                return RedirectToAction("NotFound", "Error");
            }
            

            return View(order);
        }

        public async Task<IActionResult> ListOrders(int page = 1)
        {
            int pageSize = 10; 
            var userId = GetUserId();

            if (userId is null)
            {
                SetErrorMessage("You must login");
                return RedirectToAction("NotFound", "Error");
            }

            var response = await Client.GetAsync($"/api/order-list/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to load orders");
                return RedirectToAction("NotFound", "Error");
            }

            var orderListModel = await response.Content.ReadFromJsonAsync<OrderListViewModel>();

            if (orderListModel.orders == null)
            {
                ViewBag.Error = "Orders not found";
                return RedirectToAction("NotFound", "Error");
            }

            var pagedOrders = orderListModel.orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new OrderListViewModel
            {
                orders = pagedOrders,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)orderListModel.orders.Count() / pageSize)
            };

            return View(model);
        }
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var response = await Client.GetAsync($"/api/orders/{id}");

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to load order");
                return RedirectToAction("NotFound", "Error");
            }

            var order = await response.Content.ReadFromJsonAsync<OrderEntity>();

            return View(order);

        }
        // /*
        // public async Task<IActionResult> OrderDelivered(int id)
        // {
        // var order = await _orderService.GetOrderByIdAsync(id);
        // if (order == null)
        // {
        //     ViewBag.Error = "Order not found";
        //     return RedirectToAction("NotFound", "Error");
        // }
        // order.Status = OrderStatus.Delivered;
        // var orderItems = await _orderService.GetOrderItemsByOrderIdAsync(order.Id);
        // foreach (var orderItem in orderItems)
        // {
        // await _Repository.DeleteAsync<OrderItemEntity>(orderItem.Id);
        // }
        // await _orderService.UpdateOrderAsync(order);
        // */
        public async Task<IActionResult> CreateOrder(int userId)
        {
            var dto = new OrderDTO
            {
                OrderCode = "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                UserId = userId
            };

            var response = await Client.PostAsJsonAsync("/api/create/order",dto);

            if (!response.IsSuccessStatusCode)
            {
                SetErrorMessage("Failed to create order");
                return RedirectToAction("NotFound", "Error");
            }

            var order = await response.Content.ReadFromJsonAsync<OrderEntity>();

            return RedirectToAction("OrderConfirmation", new { id = order.Id });
        }



    }
}
