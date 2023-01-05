using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Kernel.Pdf.Xobject;
using iText.Layout.Element;
using iText.StyledXmlParser.Jsoup.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CPMS_PDFReport_Marker
{
    public class MarkerTool
    {
        public PdfFont GetPDFFont(out bool status, out string errorMsg)
        {
            var result = new Paragraph();
            status = true;
            errorMsg = "";
            try
            {

                PdfFont font = PdfFontFactory.CreateFont(CPMS_PDFReport_Marker.Const.fontTTFPath, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

                return font;
            }
            catch (Exception ex)
            {
                status = false; 
                errorMsg = ex.Message;
                return null;
            }
        }
    }
}
