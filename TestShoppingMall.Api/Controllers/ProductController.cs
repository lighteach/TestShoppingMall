using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Web.Mvc;
using TestShoppingMall.Api.Dac;
using TestShoppingMall.Api.Models;

namespace TestShoppingMall.Api.Controllers
{
    public class ProductController : Controller
    {
        private ProductDac prdDac = new ProductDac();

        [HttpGet]
        public JsonResult GetProductList(int pageNo, int pageSize, string name, string category, string status)
        {
            ProductListInfo prdInfo = (new ProductDac()).GetProductList(pageNo, pageSize, name, category, status);
            return Json(prdInfo, JsonRequestBehavior.AllowGet);
        }

        [Route("Product/GetProductItem/{prdIdx?}")]
        [HttpGet]
        public JsonResult GetProductItem(int prdIdx)
        {
            ProductItem item = (new ProductDac()).GetProductItem(prdIdx);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    }
}
