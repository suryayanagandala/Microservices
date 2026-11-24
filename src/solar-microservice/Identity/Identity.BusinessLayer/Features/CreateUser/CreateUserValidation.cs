using BCrypt.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.CreateUser
{
    public class CreateUserValidation : AbstractValidator<RegisterCommand>
    {
        public CreateUserValidation()
        {
            RuleFor(x => x.userName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.email).NotEmpty().EmailAddress().WithMessage("Email is required");

            RuleFor(x => x.password)
    .NotEmpty()
    .MinimumLength(8)
    .MaximumLength(12)
    .Matches("[A-Z]")
    .WithMessage(
        "Password must contain one uppercase letter.")
    .Matches("[a-z]")
    .WithMessage(
        "Password must contain one lowercase letter.")
    .Matches("[0-9]")
    .WithMessage(
        "Password must contain one digit.")
    .Matches("[^a-zA-Z0-9]")
    .WithMessage(
        "Password must contain one special character.");
            RuleFor(x => x.userName).MaximumLength(100);

            RuleFor(x => x.phoneNumber)
     .NotEmpty()
     .WithMessage("Phone number is required.")
     .Length(10)
     .WithMessage("Phone number must be exactly 10 digits.")
     .Matches(@"^[0-9]{10}$")
     .WithMessage("Phone number must contain only digits.")
     .Must(phone =>
         long.TryParse(phone, out var number) &&
         number >= 6666666666 &&
         number <= 9999999999)
     .WithMessage("Phone number must be between 6666666666 and 9999999999.");

            RuleFor(x => x.address).NotEmpty().WithMessage("Please fill the address");

        }
    }

    public static class PasswordHash
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password,string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);    
        }
    }
}
