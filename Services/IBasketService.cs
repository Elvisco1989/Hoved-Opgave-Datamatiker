using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Hoved_Opgave_Datamatiker.Services
{
    public interface IBasketService
    {
        void AddToBasket(int customerId, Product product, int quantity);

        void UpdateItemQuantity(int customerId, int product, int quantity);

        void RemoveItem(int customerId, int productId);



        void ClearBasket(int customerId);
        List<OrderItem> GetBasket(int customerId);
    }
}