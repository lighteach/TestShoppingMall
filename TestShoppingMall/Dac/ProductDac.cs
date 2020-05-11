using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TestShoppingMall.Models;
using Newtonsoft.Json;

namespace TestShoppingMall.Dac
{
    public class ProductDac : BaseDac
    {
        public ProductDac(HttpContext cont)
        {
            WebContext = cont;
        }

        public ProductListInfo GetProductList(int pageNo, int pageSize, string name, string category, string status)
        {
            string json = LinkageToApi("/Product/GetProductList", "GET", new Dictionary<string, string>
            {
                { "pageNo", pageNo.ToString() }
                , { "pageSize", pageSize.ToString() }
                , { "name", name }
                , { "category", category }
                , { "status", status }
            });
            return JsonConvert.DeserializeObject<ProductListInfo>(json);
        }

        public ProductItem GetProductItem(int prdIdx)
        {
            string json = LinkageToApi("/Product/GetProductItem/" + prdIdx.ToString(), "GET", new Dictionary<string, string>());
            return JsonConvert.DeserializeObject<ProductItem>(json);
        }
    }
}
