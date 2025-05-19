using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using APITaller1.src.Dtos;
using APITaller1.src.Interfaces;

namespace APITaller1.src.Services
{
    public class PdfService : IPdfService
    {
        public byte[] GenerateOrderPdf(OrderDto order)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Header().Text($"Comprobante de Pedido #{order.OrderId}")
                        .FontSize(20).Bold();
                    
                    page.Content().Element(c =>
                    {
                        c.Column(col =>
                        {
                            col.Spacing(10);
                            col.Item().Text($"Fecha: {order.CreatedAt:dd/MM/yyyy HH:mm}");
                            col.Item().Text($"Estado: {order.Status}");
                            col.Item().Text($"Total: ${order.TotalAmount:N0} CLP");

                            col.Item().Text("Productos:").Bold();

                            foreach (var item in order.Items)
                            {
                                col.Item().Text(
                                    $"- {item.ProductName} x{item.Quantity} @ ${item.UnitPrice:N0} = ${item.Quantity * item.UnitPrice:N0}"
                                );
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text("Gracias por su compra - BLACKCAT E-Commerce");
                });
            });

            return document.GeneratePdf();
        }
    }
}
