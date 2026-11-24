using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.CreateProductImage
{
    public class CreateProductImageValidator:AbstractValidator<CreateProductImageRequest>
    {
        public CreateProductImageValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(x => x.UploadFiles).NotEmpty().WithMessage("Image Url is required");

        }

    }
}
