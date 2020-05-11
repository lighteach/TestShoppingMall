using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestShoppingMall.Biz;
using TestShoppingMall.Models;

namespace TestShoppingMall.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Product/Detail/{prdIdx}")]
        public IActionResult Detail(int prdIdx)
        {
            ProductItem item = (new ProductBiz(HttpContext)).GetProductItem(prdIdx);
            return View(item);
        }

        public JsonResult GetProductList(int pageNo, int pageSize, string name, string category, string status)
        {
            ProductListInfo prdInfo = (new ProductBiz(HttpContext)).GetProductList(pageNo, pageSize, name, category, status);
            
            return Json(prdInfo);
        }
    }
}