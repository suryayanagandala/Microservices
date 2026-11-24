using Microsoft.AspNetCore.Http;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.ApplicationLayer.Interfaces
{
    public interface IUtilityInterface
    {
        string GenerateSlug(string slug);

        Task<string> UploadImage(UploadFile image,string folder,CancellationToken cancellationToken);

        Task DeleteImage(string iamgeUrl, CancellationToken cancellationToken);

        string GetUnit(string capacity,int categoryId);
    }
}
