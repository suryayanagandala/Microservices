using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.CoreModels.Dto_s
{
    public record GetProductsDto
    (
        int productId,string productName,string description,decimal price,int stock,
        decimal warranty,CategoriesDto CategoriesDto,List<ProductImageDto> ImageDtos 
        );
    public record ProductImageDto(int imageId,string imageUrl);


    public record CategoriesDto(int categoryId,string categoryName,
        string description,string slug,int parentId);
}
