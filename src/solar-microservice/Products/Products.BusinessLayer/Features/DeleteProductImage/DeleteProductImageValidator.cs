using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.DeleteProductImage
{
    public class DeleteProductImageValidator:AbstractValidator<DeleteProductImageRequest>
    {
        public DeleteProductImageValidator()
        {
            RuleFor(x=>x.imageId).NotEmpty().WithMessage("Imageid required");
        }
    }
}
