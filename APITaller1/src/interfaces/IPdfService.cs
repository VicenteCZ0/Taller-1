using APITaller1.src.Dtos;

namespace APITaller1.src.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateOrderPdf(OrderDto order);
    }
}
