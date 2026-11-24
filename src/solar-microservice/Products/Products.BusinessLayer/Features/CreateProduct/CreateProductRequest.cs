using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Products.BusinessLayer.Features.CreateProduct
{
    public record CreateProductRequest
    (
        string productName,string brand,string type, int capacityValue,string capacity,decimal price,
        decimal warranty_years,int stock,string description,int categoryId
        ):ICommand<CreateProductResponse>;
}
