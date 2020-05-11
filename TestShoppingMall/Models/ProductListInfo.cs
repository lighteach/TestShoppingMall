using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestShoppingMall.Models
{
    public class ProductListInfo
    {
        public IEnumerable<ProductItem> ProductList { get; set; }
        public PagingDetailInfo PagingInfo { get; set; }
    }

    public class ProductItem
    {
        public int Idx { get; set; }
        public string InsertDate { get; set; }
        public string UpdateDate { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public int Status { get; set; }	// 0:판매대기, 1:판매중, 2:매진
        public string Category { get; set; }
    }

    public class PagingDetailInfo
    {
        public int AllCnt { get; set; }
        public int CurrentPageNo { get; set; }
    }
}
