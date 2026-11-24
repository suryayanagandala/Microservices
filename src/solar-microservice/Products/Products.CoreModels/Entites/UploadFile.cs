using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.CoreModels.Entites
{
    public sealed record UploadFile
    (Stream Stream,string fileName,string contentType,long length);

    public static class UploadFolders
    {
        public const string Categories = "categories";
        public const string Products = "products";
        public const string Users = "users";
    }

}
