using ClosedXML.Excel;
using gsbMonolith.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace gsbMonolith.Utils
{
    /// <summary>
    /// Fournit des méthodes pour importer des adhérents à partir de fichiers Excel.
    /// </summary>
    public class ExcelImporter
    {
        /// <summary>
        /// Importe une liste d'adhérents à partir d'un fichier Excel.
        /// Colonnes attendues : Nom, Prénom, Email, Téléphone, DateAdhesion, CotisationReglee.
        /// </summary>
        /// <param name="filePath">Chemin complet du fichier .xlsx</param>
        /// <returns>Une liste d'objets Adherent prêts à être insérés.</returns>
        public List<Adherent> ImportAdherents(string filePath)
        {
            var adherents = new List<Adherent>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Le fichier Excel est introuvable.");

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var lastRow = worksheet.LastRowUsed().RowNumber();
                
                // On commence à 2 pour ignorer l'entête
                for (int rowNum = 2; rowNum <= lastRow; rowNum++)
                {
                    var row = worksheet.Row(rowNum);
                    try
                    {
                        // Lecture sécurisée des cellules
                        string nom = GetStringValue(row.Cell(1));
                        string prenom = GetStringValue(row.Cell(2));
                        string email = GetStringValue(row.Cell(3));
                        string rawPhone = GetStringValue(row.Cell(4));
                        
                        // Nettoyage Téléphone
                        if (rawPhone.Length == 9 && (rawPhone.StartsWith("6") || rawPhone.StartsWith("7")))
                        {
                            rawPhone = "0" + rawPhone;
                        }

                        // Lecture Date (Sécure)
                        DateTime dateAdh = DateTime.Now;
                        var dateCell = row.Cell(5);
                        if (dateCell.DataType == XLDataType.DateTime)
                        {
                            dateAdh = dateCell.GetDateTime();
                        }
                        else if (dateCell.DataType == XLDataType.Number)
                        {
                            // Cas où Excel stocke la date comme un nombre (Serial Date)
                            try { dateAdh = dateCell.GetDateTime(); } catch { }
                        }
                        else
                        {
                            string dateStr = GetStringValue(dateCell);
                            if (!string.IsNullOrWhiteSpace(dateStr))
                            {
                                // Tentative avec plusieurs formats courants
                                string[] formats = { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "dd-MM-yyyy" };
                                if (!DateTime.TryParseExact(dateStr, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateAdh))
                                {
                                    if (!DateTime.TryParse(dateStr, out dateAdh))
                                    {
                                        dateAdh = DateTime.Now; // Fallback si tout échoue
                                    }
                                }
                            }
                        }

                        // Lecture Cotisation (Robuste)
                        bool isPaid = false;
                        var cotisCell = row.Cell(6);
                        if (cotisCell.DataType == XLDataType.Boolean)
                        {
                            isPaid = cotisCell.GetBoolean();
                        }
                        else
                        {
                            string val = GetStringValue(cotisCell).ToUpper();
                            isPaid = val == "VRAI" || val == "TRUE" || val == "1" || val == "YES" || val == "OUI";
                        }

                        if (!string.IsNullOrWhiteSpace(nom))
                        {
                            adherents.Add(new Adherent
                            {
                                Nom = nom,
                                Prenom = prenom,
                                Email = email,
                                Telephone = rawPhone,
                                DateAdhesion = dateAdh,
                                CotisationReglee = isPaid
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        // On continue pour les autres lignes mais on logue
                        System.Diagnostics.Debug.WriteLine($"Erreur ligne {rowNum}: {ex.Message}");
                    }
                }
            }

            return adherents;
        }

        private string GetStringValue(IXLCell cell)
        {
            if (cell.IsEmpty()) return "";
            try { return cell.GetValue<string>().Trim(); }
            catch { return cell.Value.ToString().Trim(); }
        }
    }
}
