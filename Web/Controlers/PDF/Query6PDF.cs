using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using GestionDeMisiones.Models;

namespace GestionDeMisiones.Web
{
    public class RelacionHechicerosDocument : IDocument
    {
        private readonly IEnumerable<Query6Result> _data;

        public RelacionHechicerosDocument(IEnumerable<Query6Result> data)
        {
            _data = data;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Text("Reporte de Hechiceros y Discipulos")
                    .FontSize(18)
                    .Bold()
                    .AlignCenter();

                page.Content().PaddingVertical(10).Element(ComposeTables);

                page.Footer().AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Generado el ");
                        x.Span(DateTime.Now.ToString("dd/MM/yyyy"));
                    });
            });
        }

        void ComposeTables(IContainer container)
        {
            foreach (var hechicero in _data)
            {
                container.Element(c =>
                {
                    c.Column(column =>
                    {
                        column.Item().Text($"{hechicero.NombreHechicero} ({hechicero.Grado})")
                            .Bold()
                            .FontSize(12);

                        column.Item().Text(
                            $"Misiones Totales: {hechicero.MisionesTotales}, " +
                            $"Éxitos: {hechicero.MisionesExitosas}, " +
                            $"Fallos: {hechicero.MisionesFallidas}, " +
                            $"% Éxito: {hechicero.PorcentajeExito:F2}%"
                        );

                        if (hechicero.Discipulos.Any())
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    cols.RelativeColumn(); // Nombre
                                    cols.RelativeColumn(); // Grado
                                    cols.RelativeColumn(); // Tipo de relación
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Discípulo").Bold();
                                    header.Cell().Text("Grado").Bold();
                                    header.Cell().Text("Tipo Relación").Bold();
                                });

                                foreach (var d in hechicero.Discipulos)
                                {
                                    table.Cell().Text(d.NombreDiscipulo);
                                    table.Cell().Text(d.GradoDiscipulo);
                                    table.Cell().Text(d.TipoRelacion);
                                }
                            });
                        }
                        else
                        {
                            column.Item().Text("No tiene discípulos asignados");
                        }

                        column.Item().PaddingVertical(5).LineHorizontal(1);
                    });
                });
            }
        }
    }
}
