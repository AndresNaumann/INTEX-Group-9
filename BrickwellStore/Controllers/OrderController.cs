using BrickwellStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrickwellStore.Controllers
{
    public class OrderController : Controller
    {
        private ILegoRepository _repo;

        public OrderController(ILegoRepository temp)
        {
            _repo = temp;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult OrderDetails(int id)
        {
            var order = _repo.Orders.FirstOrDefault(o => o.TransactionId == id);
            if (order == null)
            {
                return NotFound(); // Or handle accordingly
            }

            return View(order);
        }

        // View/Approve/Delete Orders

        [HttpGet]
        public IActionResult ViewOrder(int id)
        {
            var order = _repo.GetOrderById(id);
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ApproveOrder(int id)
        {
            var order = _repo.GetOrderById(id);
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ApproveOrder(Order order)
        {
            var orderToApprove = _repo.GetCustomerById(order.TransactionId);
            _repo.ApproveOrder(order.TransactionId);
            _repo.SaveChanges();
            
            return RedirectToAction("AdminOrders", "Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CompleteOrder(int id)
        {
            var order = _repo.GetOrderById(id);
            return View(order);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CompleteOrder(Order order)
        {
            var orderToComplete = _repo.GetOrderById(order.TransactionId);
            _repo.CompleteOrder(order.TransactionId);
            _repo.SaveChanges();

            return RedirectToAction("AdminOrders", "Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DeleteOrder(int id)
        {
            var orderToDelete = _repo.GetOrderById(id);
            return View(orderToDelete);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteOrder(Order order)
        {
            _repo.DeleteOrder(order.TransactionId);
            _repo.SaveChanges();

            return RedirectToAction("AdminOrders", "Admin");
        }
    }
}
