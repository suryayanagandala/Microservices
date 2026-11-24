using BuildingBlocks.Messaging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Contracts.Events
{
    public record CategoryEvents();

    public record DeleteCategoryEvent(int categoryId,string reason,string userId,string? message="deleted succesfully"):IntegrationEvent;

    public record CreateProductEvent(int productId,string userId,string message):IntegrationEvent;

    public record DeleteProductEvent(int productId,string userId,string message):IntegrationEvent;

    public record UpdateProductEvent(int productId, string userId, string message) : IntegrationEvent;

    public record UploadProductImageEvent(int productId,int imageId, string userId, string message) : IntegrationEvent;

    public record UpdateProductImageEvent(int productId, int imageId, string userId, string message):IntegrationEvent;

    public record DeleteProductImageEvent(int productId,int imageId, string userId, string message) : IntegrationEvent;



}
