using System;
using System.Collections.Generic;
using System.Linq;
using APITaller1.src.models;
using APITaller1.src.Dtos;

namespace APITaller1.src.Mappers
{
    public static class RoleMapper
    {
        /// <summary>
        /// Maps a Role entity to a RoleDto
        /// </summary>
        public static RoleDto ToDto(this Role role)
        {
            if (role == null)
                return null;

            return new RoleDto
            {
                RoleID = role.RoleID,
                RolName = role.RolName
            };
        }

        /// <summary>
        /// Maps a RoleDto to a Role entity
        /// </summary>
        public static Role ToEntity(this RoleDto dto)
        {
            if (dto == null)
                return null;

            return new Role
            {
                RoleID = dto.RoleID,
                RolName = dto.RolName
            };
        }

        /// <summary>
        /// Maps a collection of Role entities to a collection of RoleDto objects
        /// </summary>
        public static IEnumerable<RoleDto> ToDtoList(this IEnumerable<Role> roles)
        {
            return roles?.Select(r => r.ToDto()).ToList();
        }

        /// <summary>
        /// Updates an existing Role entity with values from a RoleDto
        /// </summary>
        public static void UpdateFromDto(this Role entity, RoleDto dto)
        {
            if (entity == null || dto == null)
                return;

            entity.RolName = dto.RolName;
        }
    }
}