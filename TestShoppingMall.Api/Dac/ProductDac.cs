using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TestShoppingMall.Api.Models;

namespace TestShoppingMall.Api.Dac
{
    public class ProductDac
    {
        private DbAccess _dbAcc = DbAccess.CreateInstance("Data Source=sql16-004.cafe24.com; Initial Catalog=zins009; User ID=zins009; Password=zinsftp1324");

        public ProductListInfo GetProductList(int pageNo, int pageSize, string name, string category, string status)
        {
            ProductListInfo prdInfo = new ProductListInfo();
            DataSet ds = _dbAcc.GetSpToDataSet("SP_Products", new Dictionary<string, string>
            {
                { "@pageNo", pageNo.ToString() }
                , { "@pageSize", pageSize.ToString() }
                , { "@Name", name }
                , { "@Category", category }
                , { "@Status", status }
            });
            if (ds.Tables.Count > 0)
            {
                prdInfo.ProductList = ds.Tables[0].Rows.Cast<DataRow>().Select(r => _dbAcc.ConvertToEntity<ProductItem>(r));
                prdInfo.PagingInfo = _dbAcc.ConvertToEntity<PagingDetailInfo>(ds.Tables[1].Rows[0]);
            }

            return prdInfo;
        }

        public ProductItem GetProductItem(int prdIdx)
        {
            ProductItem item = _dbAcc.ExecProcedureToSingle<ProductItem>("SP_ProductItem", new Dictionary<string, string>
            {
                { "@Idx", prdIdx.ToString() }
            });
            return item;
        }
    }
}