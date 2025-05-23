using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITaller1.src.Dtos;
using APITaller1.src.data;
using APITaller1.src.models;
using APITaller1.src.Mappers;

namespace APITaller1.src.Services
{
    public class CartItemService
    {
        private readonly UnitOfWork _unitOfWork;

        public CartItemService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CartItemShowDto>> GetCartItemsAsync(int userId)
        {
            var cart = await _unitOfWork.ShoppingCartRepository.GetByUserIdAsync(userId);
            if (cart == null) throw new Exception("Carrito no encontrado");

            var items = await _unitOfWork.CartItemRepository.GetByCartIdAsync(cart.ID);
            return items.Select(CartItemMapper.ToShowDto).ToList();
        }

        public async Task AddItemAsync(int userId, CartItemCreationDto dto)
        {
            var cart = await _unitOfWork.ShoppingCartRepository.GetOrCreateCartAsync(userId);

            var existingItem = await _unitOfWork.CartItemRepository
                .GetByCartAndProductAsync(cart.ID, dto.ProductID);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
                await _unitOfWork.CartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    ShoppingCartID = cart.ID,
                    ProductID = dto.ProductID,
                    Quantity = dto.Quantity
                };
                await _unitOfWork.CartItemRepository.AddAsync(newItem);
            }

            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateQuantityAsync(int userId, int productId, int quantity)
        {
            var cart = await _unitOfWork.ShoppingCartRepository.GetByUserIdAsync(userId);
            if (cart == null) throw new Exception("Carrito no encontrado");

            var item = await _unitOfWork.CartItemRepository
                .GetByCartAndProductAsync(cart.ID, productId);
            if (item == null) throw new Exception("Producto no encontrado en el carrito");

            // Validación de stock
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null) throw new Exception("Producto no existe");
            if (quantity > product.Stock)
                throw new Exception("La cantidad solicitada excede el stock disponible");

            item.Quantity = quantity;
            await _unitOfWork.CartItemRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task RemoveItemAsync(int userId, int productId)
        {
            var cart = await _unitOfWork.ShoppingCartRepository
                .GetByUserIdWithItemsAsync(userId);

            if (cart?.CartItems == null || !cart.CartItems.Any())
            {
                throw new Exception("El carrito está vacío");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductID == productId);

            if (cartItem == null)
            {
                // Verificar si el producto existe
                var productExists = await _unitOfWork.ProductRepository.Exists(productId);
                throw new Exception(productExists
                    ? $"El producto {productId} no está en tu carrito"
                    : $"El producto {productId} no existe en el sistema");
            }

            await _unitOfWork.CartItemRepository.RemoveAsync(cartItem);
            await _unitOfWork.SaveChangeAsync();

        }
        
        public async Task<decimal> GetCartTotalAsync(int userId)
        {   
            var cart = await _unitOfWork.ShoppingCartRepository.GetByUserIdAsync(userId);
            if (cart == null) throw new Exception("Carrito no encontrado");

            var items = await _unitOfWork.CartItemRepository.GetByCartIdAsync(cart.ID);
            return items.Sum(i => i.Product.Price * i.Quantity);
        }
    }
}