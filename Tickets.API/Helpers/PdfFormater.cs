using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Tickets.API.Models.Domain;
using iText.Layout.Borders;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using Tickets.API.Models.DTO.Ticket;
using System.Runtime.Serialization.Formatters.Binary;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using iText.Layout.Properties;

namespace Tickets.API.Helpers
{
    public class PdfFormater
    {
        public byte[] FormatoOrdenDeServicio(TicketDetalleDto model)
        {
            //String imageFile = "C:/logo_benavides.jpg";
            try
            {
                string asignados = "";
                foreach (var item in model.Asignados)
                {
                    asignados = asignados + item.Nombre.Trim()+", ";
                }
                
                string localizacion = model.Area;
                string solicitante = model.Solicitante;

                
                int fontSize = 9;
                var imageFile = System.IO.Path.Combine("Resources", "logo.png");
                MemoryStream ms = new MemoryStream();
                ImageData data = ImageDataFactory.Create(imageFile);
                Image img = new Image(data);

                PdfWriter pw = new PdfWriter(ms);
                PdfDocument pdfDocument = new PdfDocument(pw);
                Document doc = new Document(pdfDocument, PageSize.LETTER);
                Table table = new Table(2).UseAllAvailableWidth();

                Table tableFinal = new Table(10).UseAllAvailableWidth();
                //_cell_1_1.Add(new Paragraph("INSTITUTO MEXICANO DEL SEGURO SOCIAL\nDIVISION DE CONSERVACION\nSISTEMA DE INFORMACION-ORDEN DE SERVICIO"))
                tableFinal.AddCell(new Cell(1,8).Add(new Paragraph("INSTITUTO MEXICANO DEL SEGURO SOCIAL\nDIVISION DE CONSERVACION\nSISTEMA DE INFORMACION-ORDEN DE SERVICIO")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 2).Add(img).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 7).Add(new Paragraph("JEFATURA DE CONSERVACION DE UNIDAD: UMAE 25")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph("ORDEN DE SERVICIO")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 7).Add(new Paragraph("UMAE HOSPITAL DE ESPECIALIDADES No. 25")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph(model.Folio.ToString())).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 7).Add(new Paragraph("UNIDAD")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph("CONTRATO")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 7).Add(new Paragraph("LOCALIZACION DEL EQUIPO O INSTALACION:")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph("FECHA DE FORMULACION")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(2, 7).Add(new Paragraph(localizacion)).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("DIA")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("MES")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("AÑO")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                //tableFinal.AddCell(new Cell(1, 7).Add(new Paragraph("")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph(model.FechaCreacion.Split("/")[0])).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph(model.FechaCreacion.Split("/")[1])).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph(model.FechaCreacion.Split("/")[2])).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                
                tableFinal.AddCell(new Cell(1, 10).Add(new Paragraph("DECRIPCION DEL TRABAJO")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 10).Add(new Paragraph(model.Descripcion)).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 7).Add(new Paragraph("CODIFICACION DEL EQUIPO (KARDEX)")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph("TIEMPO ESTIMADO")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 7).Add(new Paragraph("VER A: "+ solicitante)).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph("TECNICO: " + asignados)).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(4, 3).Add(new Paragraph("REGISTRO DE LA HORA DE INICIO Y TERMINACION")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph("TIEMPO REAL (HRS. HOMB)")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 2).Add(new Paragraph("HORAS")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 2).Add(new Paragraph("MINUTOS")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph("COSTO DE MANO DE OBRA")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 4).Add(new Paragraph("$")).SetHorizontalAlignment(HorizontalAlignment.LEFT).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph("COSTO MATS. REF.")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 4).Add(new Paragraph("$")).SetHorizontalAlignment(HorizontalAlignment.LEFT).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph("COSTO TOT.")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 4).Add(new Paragraph("$")).SetHorizontalAlignment(HorizontalAlignment.LEFT).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("CANT.")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("UNIDAD")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("PRECIO UNITARIO")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 5).Add(new Paragraph("CONCEPTO")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("A/D")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("COSTO")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                for(int i = 0; i < 14; i++)
                {
                    tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("\n")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                    tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("\n")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                    tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("\n")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                    tableFinal.AddCell(new Cell(1, 5).Add(new Paragraph("\n")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                    tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("\n")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                    tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("\n")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                }
                

              
                tableFinal.AddCell(new Cell(4, 3).Add(new Paragraph("OBSERVACIONES")).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 7).Add(new Paragraph("RECIBO DE CONFORMIDAD")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 7).Add(new Paragraph("NOMBRE")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("DIA")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 2).Add(new Paragraph("MES")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("AÑO")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph("FIRMA")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("\n\n")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 2).Add(new Paragraph("\n\n")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 1).Add(new Paragraph("\n\n")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));
                tableFinal.AddCell(new Cell(1, 3).Add(new Paragraph("\n\n")).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(fontSize));

                doc.Add(tableFinal);
                doc.SetFontSize(9);
                doc.Close();

                byte[] bytes = ms.ToArray();
                ms = new MemoryStream(); ;
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;




                return ms.ToArray();

                



                //pdfDocument.SetDefaultPageSize




            }
            catch(Exception ioe)
            {
                return null;
            }
            
        }
    }
}
