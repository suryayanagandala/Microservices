using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.CreateProduct
{
    public class CreateProductValidator:AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.productName)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

            RuleFor(x => x.categoryId)
                .GreaterThan(0).WithMessage("Please select a valid category.");

            RuleFor(x => x.brand)
                .NotEmpty().WithMessage("Brand is required.")
                .MaximumLength(200).WithMessage("Brand cannot exceed 200 characters.");

            RuleFor(x => x.type)
                .NotEmpty().WithMessage("Type is required.")
                .MaximumLength(100).WithMessage("Type cannot exceed 100 characters.");

            RuleFor(x => x.capacityValue)
                .GreaterThan(0).WithMessage("Capacity value must be greater than 0.");

            RuleFor(x => x.capacity)
                .NotEmpty().WithMessage("Capacity is required.")
                .MaximumLength(10).WithMessage("Capacity cannot exceed 10 characters.");

            RuleFor(x => x.price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.warranty_years)
                .GreaterThanOrEqualTo(0).WithMessage("Warranty years cannot be negative.");

            RuleFor(x => x.stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

            RuleFor(x => x.description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters.");

            
        }
    }
}
