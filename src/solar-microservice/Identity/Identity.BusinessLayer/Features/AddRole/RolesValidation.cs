using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.AddRole
{
    public class RolesValidation :AbstractValidator<RoleHandlerRequest>
    {
        public RolesValidation()
        {
          

            RuleFor(x => x.roleName).NotEmpty().WithMessage("RoleName is required");

           
        }
    }
}
