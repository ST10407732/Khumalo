using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using KhumaloCraftKC.Models;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis;

namespace KhumaloCraftKC.Controllers
{
    public class OrderController : Controller
    {
        private readonly KhumaloDbContext _context;

        public OrderController(KhumaloDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            // Fetch all orders
            var orders = await _context.Orders
                .Include(o => o.Product)
                .ToListAsync();
            return View(orders);
        }

        // GET: Order/Create
        public IActionResult Create(int productId)
        {
            ViewBag.ProductId = productId;
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            var order = new Order
            {
                ProductId = product.ProductId,
                Product = product
            };

            return View(order);
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            // Placeholder user ID for demonstration purposes
            var userId = 1; // Replace with actual user ID logic if needed

            var order = new Order
            {
                ProductId = productId,
                UserId = userId,
                Quantity = quantity,
                OrderDate = DateTime.Now,
                TotalPrice = product.Price * quantity
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Order/OrderHistory
        public async Task<IActionResult> OrderHistory()
        {
            // Placeholder user ID for demonstration purposes
            var userId = 1; // Replace with actual user ID logic if needed

            var orderHistory = await _context.Orders
                .Include(o => o.Product)
                .Where(o => o.UserId == userId && o.OrderDate < DateTime.Now)
                .ToListAsync();

            return View(orderHistory);
        }
        // GET: Order/Checkout
        public IActionResult Checkout(DeliveryOptionsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var deliveryOptionsJson = JsonConvert.SerializeObject(model);
                TempData["DeliveryOptions"] = deliveryOptionsJson; // Store delivery details in TempData as JSON string
                return RedirectToAction("Index", "Transaction"); // Redirect to transaction index
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            // Find the order item in the database based on the product ID
            var orderItem = _context.Orders.FirstOrDefault(o => o.ProductId == productId);

            if (orderItem == null)
            {
                // Handle the case where the item is not found
                return NotFound(); // You can return a 404 Not Found status code or handle it differently
            }

            // Remove the order item from the database
            _context.Orders.Remove(orderItem);
            _context.SaveChanges(); // Save changes to persist the removal

            // Redirect the user back to the cart or any other appropriate page
            return RedirectToAction("Index"); // For example, redirect to the cart page
        }

    }
}
