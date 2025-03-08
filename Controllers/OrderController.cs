using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;

namespace MyApp.Namespace
{
    public class OrderController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        public OrderController(IUserService userService, IProductService productService, IOrderService orderService)
        {
            _userService = userService;
            _productService = productService;
            _orderService = orderService;
        }
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                ViewBag.Error = "Order not found";
                return View();
            }
            await _orderService.DeleteOrderAsync(orderId);
            return RedirectToAction("ListOrders");
        }
        public async Task<IActionResult> OrderDetails(int? orderId)
        {
            int userId = _userService.GetUserId();
            if(userId == 0)
            {
                return RedirectToAction("NotFound", "Error");
            }
            if (!orderId.HasValue || orderId.Value <= 0)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var order = await _orderService.GetOrderByIdAsync(orderId.Value);

            if (order == null)
            {
                ViewBag.Error = "Order not found";
                return RedirectToAction("NotFound", "Error");
            }
            if(userId != order.UserId)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var orderItems = await _orderService.GetOrderItemsByOrderIdAsync(order.Id);
            var products = await _productService.ListAllProducts();
            ProductEntity product = null;

            foreach (var orderItem in orderItems)
            {
                product = products.FirstOrDefault(p => p.Id == orderItem.ProductId);
                if (product != null)
                {
                    break; 
                }
            }

            return View(new OrderDetailsViewModel
            {
                order = order,
                product = product,
                orderItems = orderItems.ToList()
            });
        }

        public async Task<IActionResult> ListOrders(int page = 1)
        {
            int pageSize = 10; 
            int userId = _userService.GetUserId();
            var user = await _userService.GetUserAsync(userId);
            if (!user.Success)
            {
                ViewBag.Error = user.Message;
                return RedirectToAction("NotFound", "Error");
            }


            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            var orderItems = await _orderService.GetAllOrderItems();

            var pagedOrders = orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new OrderListViewModel
            {
                orders = pagedOrders,
                orderItems = orderItems.ToList(),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)orders.Count() / pageSize)
            };

            return View(model);
        }
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return View(order);

        }
        /*
        public async Task<IActionResult> OrderDelivered(int id)
        {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            ViewBag.Error = "Order not found";
            return RedirectToAction("NotFound", "Error");
        }
        order.Status = OrderStatus.Delivered;
        var orderItems = await _orderService.GetOrderItemsByOrderIdAsync(order.Id);
        foreach (var orderItem in orderItems)
        {
        await _Repository.DeleteAsync<OrderItemEntity>(orderItem.Id);
        }
        await _orderService.UpdateOrderAsync(order);
        */
        public async Task<IActionResult> CreateOrder(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId); 
            if (user == null)
            {
                return NotFound(); 
            }

            var order = await _orderService.CreateOrderAsync(new OrderDTO
            {
                UserId = userId,
                Address = user.Address,
                OrderCode = $"#{GenerateHelper.GenerateNumber()}"
            });
            
            return RedirectToAction("OrderConfirmation", "Order", new { id = order.Id }); 
        }



    }
}
