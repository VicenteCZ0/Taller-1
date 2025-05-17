using APITaller1.src.DTOs;
using APITaller1.src.models;

namespace APITaller1.src.Mappers
{
    public static class StatusMapper
    {
        public static StatusDto ToDto(Status status)
        {
            return new StatusDto
            {
                StatusID = status.StatusID,
                StatusName = status.StatusName
            };
        }
    }
}
