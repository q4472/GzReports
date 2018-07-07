using FarmSib.Base.Data;
using FarmSib.Base.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FarmSib.Base.Controllers
{
    public class ItemsController : Controller
    {
        public String GetTnItems(Guid session_id, String srcList, String searchTerm, String mnn, Boolean exactly, Int32 pageNumber)
        {
            Boolean eRu = false;
            String[] srcs = Utility.UnEscape(srcList).Split(',');
            String[] terms = Utility.UnEscape(searchTerm).Split(' ');
            String _mnn = Utility.UnEscape(mnn);
            System.Data.DataSet ds = HomeData.Home.GetTnItems(session_id, srcs, terms, _mnn, exactly, eRu, pageNumber);
            return Nskd.Json.ToString(ds);
        }
        public String GetLP(String rn)
        {
            rn = Utility.UnEscape(rn);
            System.Data.DataSet ds = HomeData.Home.GetTnItemsLP(rn);
            return Nskd.Json.ToString(ds);
        }
    }
}
