using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using P2FixAnAppDotNetCode.Models;
using P2FixAnAppDotNetCode.Models.Services;

namespace P2FixAnAppDotNetCode.Controllers
{
    public class CartController : Controller
    {
        private readonly ICart _cart;
        private readonly IProductService _productService;
        private readonly IStringLocalizer<CartController> _localizer;

        public CartController(ICart pCart, IProductService productService, IStringLocalizer<CartController> localizer)
        {
            _cart = pCart;
            _productService = productService;
            _localizer = localizer;
        }

        public ViewResult Index()
        {
            return View(_cart as Cart);
        }

        [HttpPost]
        public RedirectToActionResult AddToCart(int id)
        {
            Product product = _productService.GetProductById(id);

            if (product == null) return RedirectToAction("Index", "Product");

            if (_cart.AddItem(product, 1)) return RedirectToAction("Index");

            TempData["StockError"] = _localizer["StockError", product.Name, product.Stock].Value;
            //TempData["stockError"] = $"Sorry, your cart reached the max quantity for {product.Name} (current stock: {product.Stock}).";

            return RedirectToAction("Index", "Product");
        }

        public RedirectToActionResult RemoveFromCart(int id)
        {
            Product product = _productService.GetAllProducts()
                .FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                _cart.RemoveLine(product);
            }
            return RedirectToAction("Index");
        }
    }
}
