using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.CreateCategory
{
    public class CreateCategoryValidators : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryValidators()
        {
            RuleFor(x => x.categoryName).Must(x=>!string.IsNullOrWhiteSpace(x)).NotEmpty().MaximumLength(200).WithMessage("CategoryName is required");

            //RuleFor(x=>x.slug).Must(x => !string.IsNullOrWhiteSpace(x)).NotEmpty().MaximumLength(100).WithMessage("Slug is rquired");

            RuleFor(x => x.parentCatgoryId).GreaterThan(0).When(x => x.parentCatgoryId.HasValue);

            RuleFor(x => x.description).Must(x => !string.IsNullOrWhiteSpace(x)).MinimumLength(10).NotEmpty().WithMessage("Description is required more than 10 characters");
            //RuleFor(x=>x.image).When(x=>x.parentCatgoryId.HasValue).WithMessage("Image is required");
        }
    }
}
