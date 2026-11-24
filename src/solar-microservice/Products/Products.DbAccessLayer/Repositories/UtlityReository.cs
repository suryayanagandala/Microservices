
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Products.ApplicationLayer.Interfaces;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Products.DbAccessLayer.Repositories
{
    public sealed class UtlityReository(IWebHostEnvironment webHostEnvironment) : IUtilityInterface
    {
        //read only does not replace the hashset object but it changes the values add or remove 
        private static readonly HashSet<int> units = [35, 36, 37, 38, 39];

        private static readonly IReadOnlySet<int> unit = new HashSet<int>
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10
        };

        private static readonly ImmutableHashSet<int> un = ImmutableHashSet.Create(12, 34, 3);
        public Task DeleteImage(string imageUrl, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return Task.CompletedTask;

            var physicalPath = Path.Combine(
                webHostEnvironment.WebRootPath,
                imageUrl.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }

            return Task.CompletedTask;
        }

        public string GenerateSlug(string slug)
        {
            return slug.Trim().ToLowerInvariant().Replace(" ","-");
        }

        public  string GetUnit(string capacity, int categoryId)
        {
            

            if (units.Contains(categoryId))
            {
                return "Unit";
            }

            var match =  Regex.Match(capacity, "[A-za-z]+");

            return  match.Success ? match.Value : capacity;
        }

        public async Task<string> UploadImage(UploadFile image, string folder, CancellationToken cancellationToken)
        {
            var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads", folder);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var extension=Path.GetExtension(uploadFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            var fullPath = Path.Combine(uploadFolder, uniqueFileName);

            await using var fileStream = new FileStream(fullPath, FileMode.Create);

            await image.Stream.CopyToAsync(fileStream, cancellationToken);

            // URL stored in DB
            return $"uploads/{folder}/{uniqueFileName}";
        }

        //public void ut()
        //{
        //    units.Add(23);

        //    unit.add()

        //        //The variable cannot be assigned a different object after initialization.
        //    units = new HashSet<int>
        //    {
        //        12,23
        //    };

        //    un.add(12)
        //}
    }
}
