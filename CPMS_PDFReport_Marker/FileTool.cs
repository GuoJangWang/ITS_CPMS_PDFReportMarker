using static iText.Kernel.Colors.ColorConstants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Kernel.Pdf.Xobject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Path = System.IO.Path;
using iText.Kernel.Colors;
using System.IO;

namespace CPMS_PDFReport_Marker
{
    public class FileTool
    {
        public PdfReader ReadFile(string filePath,string fileName, out string errorMsg,out bool status)
        {
			
			errorMsg = "";
			status = true;
			try
			{
			    var fullPath =	Path.Combine(filePath,fileName);

                PdfReader result = new PdfReader(fullPath);
                return result;
            }
			catch (Exception ex)
			{
				status = false;
				errorMsg = ex.Message;
				return null;
			}
            
        }

		public void WriteFile(MemoryStream successFile,string filePath,string fileName,out string errorMsg,out bool status)
		{
            errorMsg = "";
            status = true;
            try
			{
				var fullPath = Path.Combine(filePath, fileName);

				var fileStream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write);

				successFile.WriteTo(fileStream);

			}
			catch (Exception ex)
			{
				errorMsg=ex.Message;
				status = false;
			}
		}

        public bool MakeNewPDFFileWithMarker(out string errorMsg, PdfReader originFile, PdfFont fontString,string fileName,string filePath,string markerMsg)
        {
            errorMsg = "";
            var result = true;
            try
            {

                var fullPath = Path.Combine(filePath, fileName);

                var memoryStream = new MemoryStream();

                PdfWriter pdfWriter = new PdfWriter(memoryStream);


                float watermarkTrimmingRectangleWidth = 300;
                float watermarkTrimmingRectangleHeight = 300;

                float formWidth = 300;
                float formHeight = 300;
                float formXOffset = 0;
                float formYOffset = 0;

                float xTranslation = 50;
                float yTranslation = 25;
                double rotationInRads = Math.PI / 3;

                float fontSize = 50;

                //PdfDocument pdfDoc = new PdfDocument(originFile, new PdfWriter(fullPath));
                PdfDocument pdfDoc = new PdfDocument(originFile, pdfWriter);
                

                var numberOfPages = pdfDoc.GetNumberOfPages();
                PdfPage page = null;



                for (var i = 1; i <= numberOfPages; i++)
                {
                    page = pdfDoc.GetPage(i);
                    Rectangle ps = page.GetPageSize();

                    //Center the annotation
                    float bottomLeftX = ps.GetWidth() / 2 - watermarkTrimmingRectangleWidth / 2;
                    float bottomLeftY = ps.GetHeight() / 2 - watermarkTrimmingRectangleHeight / 2;
                    Rectangle watermarkTrimmingRectangle = new Rectangle(bottomLeftX, bottomLeftY, watermarkTrimmingRectangleWidth, watermarkTrimmingRectangleHeight);

                    PdfWatermarkAnnotation watermark = new PdfWatermarkAnnotation(watermarkTrimmingRectangle);

                    //Apply linear algebra rotation math
                    //Create identity matrix
                    AffineTransform transform = new AffineTransform();//No-args constructor creates the identity transform
                                                                      //Apply translation
                    transform.Translate(xTranslation, yTranslation);
                    //Apply rotation
                    transform.Rotate(rotationInRads);

                    PdfFixedPrint fixedPrint = new PdfFixedPrint();
                    watermark.SetFixedPrint(fixedPrint);
                    //Create appearance
                    Rectangle formRectangle = new Rectangle(formXOffset, formYOffset, formWidth, formHeight);

                    //Observation: font XObject will be resized to fit inside the watermark rectangle
                    PdfFormXObject form = new PdfFormXObject(formRectangle);
                    PdfExtGState gs1 = new PdfExtGState().SetFillOpacity(0.6f);
                    PdfCanvas canvas = new PdfCanvas(form, pdfDoc);

                    float[] transformValues = new float[6];
                    transform.GetMatrix(transformValues);
                    canvas.SaveState()
                        .BeginText().SetColor(ColorConstants.GRAY, true).SetExtGState(gs1)
                        .SetTextMatrix(transformValues[0], transformValues[1], transformValues[2], transformValues[3], transformValues[4], transformValues[5])
                        .SetFontAndSize(fontString, fontSize)
                        .ShowText(markerMsg)
                        .EndText()
                        .RestoreState();

                    canvas.Release();

                    watermark.SetAppearance(PdfName.N, new PdfAnnotationAppearance(form.GetPdfObject()));
                    watermark.SetFlags(PdfAnnotation.PRINT);

                    page.AddAnnotation(watermark);

                }

                page?.Flush();

                pdfDoc.Close();

                byte[] byteA = memoryStream.ToArray();

                using (FileStream fs = File.Create(fullPath))
                {
                    fs.Write(byteA, 0, byteA.Length);
                }

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                result = false;
            }
            return result;
        }

    }
}
