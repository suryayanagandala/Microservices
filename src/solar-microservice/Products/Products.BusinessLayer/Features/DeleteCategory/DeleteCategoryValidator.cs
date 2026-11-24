using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.DeleteCategory
{
    public class DeleteCategoryValidator:AbstractValidator<DeleteCategoryRequest>
    {
        public DeleteCategoryValidator()
        {
            RuleFor(x => x.categoryId).NotEmpty().WithMessage("CategoryId is required");

            RuleFor(x => x.reason).NotEmpty().MinimumLength(10).WithMessage("Reason is required which has more than 10 chars");

        }
    }
}
