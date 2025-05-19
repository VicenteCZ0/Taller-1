using System;
using System.Collections.Generic;
using System.Linq;
using APITaller1.src.models;
using APITaller1.src.Dtos;

namespace APITaller1.src.Mappers
{
    public static class ShippingAddressMapper
    {
        /// <summary>
        /// Maps a ShippingAddress entity to a ShippingAddressDto
        /// </summary>
        public static ShippingAddressDto ToDto(this ShippingAddress address)
        {
            if (address == null)
                return null;

            return new ShippingAddressDto
            {
                AddressID = address.AddressID,
                Street = address.Street,
                Number = address.Number,
                Commune = address.Commune,
                Region = address.Region,
                PostalCode = address.PostalCode,
                UserId = address.UserId
            };
        }

        /// <summary>
        /// Maps a ShippingAddressDto to a ShippingAddress entity
        /// </summary>
        public static ShippingAddress ToEntity(this ShippingAddressDto dto)
        {
            if (dto == null)
                return null;

            return new ShippingAddress
            {
                AddressID = dto.AddressID,
                Street = dto.Street,
                Number = dto.Number,
                Commune = dto.Commune,
                Region = dto.Region,
                PostalCode = dto.PostalCode,
                UserId = dto.UserId
            };
        }

        /// <summary>
        /// Maps a collection of ShippingAddress entities to a collection of ShippingAddressDto objects
        /// </summary>
        public static IEnumerable<ShippingAddressDto> ToDtoList(this IEnumerable<ShippingAddress> addresses)
        {
            return addresses?.Select(a => a.ToDto()).ToList();
        }

        /// <summary>
        /// Updates an existing ShippingAddress entity with values from a ShippingAddressDto
        /// </summary>
        public static void UpdateFromDto(this ShippingAddress entity, ShippingAddressDto dto)
        {
            if (entity == null || dto == null)
                return;

            entity.Street = dto.Street;
            entity.Number = dto.Number;
            entity.Commune = dto.Commune;
            entity.Region = dto.Region;
            entity.PostalCode = dto.PostalCode;
            entity.UserId = dto.UserId;
        }
    }
}