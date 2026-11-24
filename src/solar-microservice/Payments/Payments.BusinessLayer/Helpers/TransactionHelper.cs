using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BusinessLayer.Helpers
{
    public static class TransactionHelper
    {
        public static string Generate()
        {
            return $"TXN{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }
    }
}
