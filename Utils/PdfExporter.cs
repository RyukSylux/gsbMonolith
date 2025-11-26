using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using gsbMonolith.Models;

// iText 9
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace gsbMonolith.Utils
{
    /// <summary>
    /// Provides utilities for exporting various documents to PDF format.
    /// </summary>
    public static class PdfExporter
    {
        /// <summary>
        /// Generates and exports a medical prescription as a PDF file.
        /// </summary>
        /// <param name="presc">The prescription object containing metadata such as the date.</param>
        /// <param name="patient">The patient for whom the prescription is created.</param>
        /// <param name="doctor">The doctor issuing the prescription.</param>
        /// <param name="meds">A list of tuples containing medicine information and the corresponding quantity.</param>
        /// <remarks>
        /// This method uses a <see cref="SaveFileDialog"/> to let the user choose where to save the PDF.
        /// It also handles null values in patient/doctor/medicine fields to avoid crashes.
        /// If an exception occurs, a message box will alert the user and the error will be logged to console.
        /// </remarks>
        public static void ExportPrescription(
            Prescription presc,
            Patient patient,
            User doctor,
            List<(Medicine med, int quantity)> meds)
        {
            try
            {
                // SAVE FILE DIALOG
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Enregistrer l'ordonnance";
                dialog.Filter = "PDF (*.pdf)|*.pdf";
                dialog.FileName = $"Ordonnance_{(patient.Name ?? "Patient")}_{(patient.Firstname ?? "")}_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                string filePath = dialog.FileName;

                // SAFE DATE PARSING
                DateTime parsedDate;
                if (!DateTime.TryParse(presc?.Date, out parsedDate))
                    parsedDate = DateTime.Now;

                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                using (PdfWriter writer = new PdfWriter(filePath))
                using (PdfDocument pdf = new PdfDocument(writer))
                using (Document doc = new Document(pdf))
                {
                    // TITLE
                    doc.Add(new Paragraph("Ordonnance médicale")
                        .SetFont(boldFont)
                        .SetFontSize(22)
                        .SetTextAlignment(TextAlignment.CENTER));

                    doc.Add(new Paragraph("\n"));

                    // PATIENT & DOCTOR INFO
                    string patientName = (patient?.Name ?? "[Nom inconnu]") + " " + (patient?.Firstname ?? "");
                    string doctorName = (doctor?.Name ?? "[Médecin]") + " " + (doctor?.FirstName ?? "");

                    doc.Add(new Paragraph($"Patient : {patientName}"));
                    doc.Add(new Paragraph($"Médecin : Dr {doctorName}"));
                    doc.Add(new Paragraph($"Validité : {parsedDate:dd/MM/yyyy}"));

                    doc.Add(new Paragraph("\n"));

                    // TABLE
                    float[] widths = { 4, 1 };
                    Table table = new Table(UnitValue.CreatePercentArray(widths)).UseAllAvailableWidth();

                    table.AddHeaderCell(new Cell().Add(new Paragraph("Médicament")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Quantité")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                    foreach (var entry in meds)
                    {
                        Medicine med = entry.med;
                        int qty = entry.quantity;

                        string medNameSafe = med?.Name ?? "[Nom manquant]";
                        string dosageSafe = med?.Dosage ?? "";
                        string display = $"{medNameSafe} {dosageSafe} mg".Trim();

                        table.AddCell(new Cell().Add(new Paragraph(display)));
                        table.AddCell(new Cell().Add(new Paragraph(qty.ToString())));
                    }

                    doc.Add(table);

                    doc.Add(new Paragraph("\n\nSignature du médecin :\n\n"));

                    doc.Close();
                }

                // OPEN THE FILE
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erreur lors de la génération du PDF :\n" + ex.Message,
                    "Erreur PDF",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                Console.WriteLine("PDF Export Error: " + ex.ToString());
            }
        }
    }
}