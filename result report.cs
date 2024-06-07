using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using static madarsa_student_app.dashboard;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.IO;
using Aspose.Pdf;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using Aspose.Pdf.Text;

namespace madarsa_student_app
{
    public partial class result_report : Form
    {
        private string ConnectionString = dashboard.ConnectionString;
        string Paper = showresult.Paper;
        string Nam = showresult.Nam;
        string klas = showresult.KLas;
        string father = showresult.father;
        string address = "";
        string m1 = showresult.m1;
        string m2 = showresult.m2;
        string m3 = showresult.m3;
        string m4 = showresult.m4;
        string m5 = showresult.m5;
        string m6 = showresult.m6;
        string m7 = showresult.m7;

        public result_report()
        {
            InitializeComponent();
        }

        private T ExecuteScalarQuery<T>(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Execute the query and return the scalar result
                object result = command.ExecuteScalar();

                // Convert the result to the specified type (T)
                return (result == DBNull.Value || result == null) ? default : (T)Convert.ChangeType(result, typeof(T));
            }
        }
        private void result_report_Load(object sender, EventArgs e)
        {
            address = ""; // Declare the variable outside the scope

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                // Query to fetch the column names and student's address
                string classQuery = @"
SELECT c.subject1, c.subject2, c.subject3, c.subject4, c.subject5, c.subject6, c.subject7, sa.address 
FROM class c 
JOIN student_admission sa ON c.id = sa.id 
WHERE c.class = @klas";

                using (SqlCommand classCommand = new SqlCommand(classQuery, sqlConnection))
                {
                    classCommand.Parameters.AddWithValue("@klas", klas);

                    using (SqlDataReader classReader = classCommand.ExecuteReader())
                    {
                        if (classReader.Read())
                        {
                            // Retrieve subject names and address, handling possible null values
                            string sub1 = classReader.IsDBNull(0) ? "" : classReader.GetString(0);
                            string sub2 = classReader.IsDBNull(1) ? "" : classReader.GetString(1);
                            string sub3 = classReader.IsDBNull(2) ? "" : classReader.GetString(2);
                            string sub4 = classReader.IsDBNull(3) ? "" : classReader.GetString(3);
                            string sub5 = classReader.IsDBNull(4) ? "" : classReader.GetString(4);
                            string sub6 = classReader.IsDBNull(5) ? "" : classReader.GetString(5);
                            string sub7 = classReader.IsDBNull(6) ? "" : classReader.GetString(6);
                            address = classReader.IsDBNull(7) ? "" : classReader.GetString(7); // Assign address to the variable

                            // Clear existing rows in the DataGridView
                            dataGridView1.Rows.Clear();

                            // Add columns with column names
                            dataGridView1.Columns.Add("Column1", "Index");
                            dataGridView1.Columns.Add("Column2", "Subject");
                            dataGridView1.Columns.Add("Column3", "Number");

                            // Add rows with initial values set to "0"
                            if (!string.IsNullOrEmpty(sub1)) dataGridView1.Rows.Add(1, sub1, m1);
                            if (!string.IsNullOrEmpty(sub2)) dataGridView1.Rows.Add(2, sub2, m2);
                            if (!string.IsNullOrEmpty(sub3)) dataGridView1.Rows.Add(3, sub3, m3);
                            if (!string.IsNullOrEmpty(sub4)) dataGridView1.Rows.Add(4, sub4, m4);
                            if (!string.IsNullOrEmpty(sub5)) dataGridView1.Rows.Add(5, sub5, m5);
                            if (!string.IsNullOrEmpty(sub6)) dataGridView1.Rows.Add(6, sub6, m6);
                            if (!string.IsNullOrEmpty(sub7)) dataGridView1.Rows.Add(7, sub7, m7);
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1 != null && dataGridView1.Rows != null && dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "Final PMS Report.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Check if the file exists and delete it
                        if (File.Exists(sfd.FileName))
                        {
                            File.Delete(sfd.FileName);
                        }

                        // Create PDF document and table
                        using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                        {
                            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 20f, 20f, 10f);
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();

                            // Add report heading
                            Paragraph heading = new Paragraph("Final Report of Poultry Management System", FontFactory.GetFont(FontFactory.TIMES_BOLD, 20f));
                            heading.Alignment = Element.ALIGN_CENTER;
                            pdfDoc.Add(heading);

                            // Add spacing
                            pdfDoc.Add(new Paragraph("\n"));

                            // Add date printed
                            Phrase printedDatePhrase = new Phrase();
                            printedDatePhrase.Add(new Chunk("Date Printed: ", FontFactory.GetFont(FontFactory.TIMES_BOLD, 14f)));
                            printedDatePhrase.Add(new Chunk(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                            Paragraph printedDate = new Paragraph(printedDatePhrase);
                            printedDate.Alignment = Element.ALIGN_RIGHT;
                            pdfDoc.Add(printedDate);

                            // Add spacing
                            pdfDoc.Add(new Paragraph("\n"));

                            Paragraph studentName = new Paragraph();
                            studentName.Alignment = Element.ALIGN_RIGHT; // Align to the right



                            string urduFontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fonts", "Jameel Noori Nastaleeq Kasheeda.ttf");
                            FontFactory.Register(urduFontPath, "Jameel Noori Nastaleeq Kasheeda");
                            // Add the actual name in non-bold
                            Chunk nameChunk = new Chunk(Nam, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15f));
                            studentName.Add(nameChunk);

                            Chunk nameLabel = new Chunk(" :نام ", FontFactory.GetFont("Jameel Noori Nastaleeq Kasheeda", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 18f));
                            studentName.Add(nameLabel);

                            pdfDoc.Add(studentName);

                            // Add student address
                            // Address paragraph
                            Paragraph addressParagraph = new Paragraph();
                            addressParagraph.Alignment = Element.ALIGN_RIGHT; // Align to the right


                            // Add the actual address in non-bold
                            Chunk addressChunk = new Chunk(address, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15f));
                            addressParagraph.Add(addressChunk);

                            Chunk addressLabel = new Chunk(" :پتہ ", FontFactory.GetFont("Jameel Noori Nastaleeq Kasheeda", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 18f));
                            addressParagraph.Add(addressLabel);

                            pdfDoc.Add(addressParagraph);

                            // Father's name paragraph
                            Paragraph fatherNameParagraph = new Paragraph();
                            fatherNameParagraph.Alignment = Element.ALIGN_RIGHT; // Align to the right

                            // Add the actual father's name in non-bold
                            Chunk fatherNameChunk = new Chunk(father, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15f));
                            fatherNameParagraph.Add(fatherNameChunk);

                            Chunk fatherNameLabel = new Chunk(" :ولدیت ", FontFactory.GetFont("Jameel Noori Nastaleeq Kasheeda", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 18f));
                            fatherNameParagraph.Add(fatherNameLabel);

                            pdfDoc.Add(fatherNameParagraph);


                            // Add spacing
                            pdfDoc.Add(new Paragraph("\n"));

                            // Create PDF table for data
                            PdfPTable pdfTable = new PdfPTable(dataGridView1.Columns.Count);
                            pdfTable.DefaultCell.Padding = 4;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            // Set relative column widths (example values)
                            int columnCount = dataGridView1.Columns.Count;
                            float[] columnWidths = new float[columnCount];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnWidths[i] = 2f; // Adjust the widths as needed
                            }
                            pdfTable.SetWidths(columnWidths);

                            // Add column headers to PDF table
                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, FontFactory.GetFont(FontFactory.TIMES_BOLD, 14f)));
                                pdfTable.AddCell(cell);
                            }

                            // Add rows and cells to PDF table
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    if (cell.Value is DateTime)
                                    {
                                        pdfTable.AddCell(((DateTime)cell.Value).ToString("MM/dd/yyyy"));
                                    }
                                    else
                                    {
                                        PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value?.ToString() ?? "", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f)));
                                        pdfTable.AddCell(pdfCell);
                                    }
                                }
                            }

                            // Add PDF table to document and close the document
                            pdfDoc.Add(pdfTable);

                            // Add space after DataGridView
                            pdfDoc.Add(new Paragraph("\n"));

                            // Add footer text
                            Paragraph footer = new Paragraph("Design and Develop By: Umar and Faeez", FontFactory.GetFont(FontFactory.TIMES_BOLD, 20f));
                            footer.Alignment = Element.ALIGN_CENTER;
                            pdfDoc.Add(footer);

                            // Add space
                            pdfDoc.Add(new Paragraph("\n"));

                            // Create a new Paragraph for the centered links
                            Paragraph centeredLinks = new Paragraph();
                            centeredLinks.Alignment = Element.ALIGN_CENTER; // Center-align the paragraph horizontally

                            // Create a Chunk containing the first link
                            Chunk link1 = new Chunk("Visit Umar's Linktree", FontFactory.GetFont(FontFactory.TIMES_BOLD, 16f));
                            link1.SetAnchor("https://linktr.ee/umarfarooq003"); // Set the link URL
                            link1.SetAction(new iTextSharp.text.pdf.PdfAction(new Uri("https://linktr.ee/umarfarooq003")));
                            // Add the first link Chunk to the centered paragraph
                            centeredLinks.Add(link1);
                            centeredLinks.Add(new Phrase("\n")); // Add spacing between the links

                            // Create a Chunk containing the second link
                            Chunk link2 = new Chunk("Visit Faeez's Linktree", FontFactory.GetFont(FontFactory.TIMES_BOLD, 16f));
                            link2.SetAnchor("https://linktr.ee/faaez_usmani"); // Set the link URL
                            link2.SetAction(new iTextSharp.text.pdf.PdfAction(new Uri("https://linktr.ee/faaez_usmani")));

                            // Add the second link Chunk to the centered paragraph
                            centeredLinks.Add(link2);

                            // Add the centered paragraph with clickable links to the PDF document
                            pdfDoc.Add(centeredLinks);

                            pdfDoc.Close();

                            MessageBox.Show("Data Exported Successfully to PDF!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Open the saved PDF file in the default program (typically a web browser)
                            Process.Start(sfd.FileName);
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Error: Unable to access file. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
