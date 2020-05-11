using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TestShoppingMall.Models;
using TestShoppingMall.Dac;
using Newtonsoft.Json;

namespace TestShoppingMall.Biz
{
    public class ProductBiz
    {
        private HttpContext _cont = null;
        public ProductBiz(HttpContext cont)
        {
            _cont = cont;
        }

        public ProductListInfo GetProductList(int pageNo, int pageSize, string name, string category, string status)
        {
            return (new ProductDac(_cont)).GetProductList(pageNo, pageSize, name, category, status);
        }

        public ProductItem GetProductItem(int prdIdx)
        {
            return (new ProductDac(_cont)).GetProductItem(prdIdx);
        }
    }
}
