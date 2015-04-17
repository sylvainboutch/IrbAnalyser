using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System;

namespace IrbAnalyser
{
    /// <summary>
    /// FROM http://www.c-sharpcorner.com/uploadfile/deveshomar/exporting-datatable-to-excel-in-c-sharp-using-interop/
    /// </summary>

    class ExcelWorksheet
    {
        public string worksheetName;
        public string reporType;
        public DataTable dataTable;

        public ExcelWorksheet(string wk,string rt,DataTable dt)
        {
            worksheetName = wk;
            reporType = rt;
            dataTable = dt;
        }
    }

    class ExcelUtility
    {
        Microsoft.Office.Interop.Excel.Application excel;
        Microsoft.Office.Interop.Excel.Workbook excelworkBook;
        Microsoft.Office.Interop.Excel.Worksheet excelSheet;
        Microsoft.Office.Interop.Excel.Range excelCellrange;

        public bool WriteDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation, string ReporType)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelworkBook;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet;
            Microsoft.Office.Interop.Excel.Range excelCellrange;

            try
            {
                // Start Excel and get Application object.
                excel = new Microsoft.Office.Interop.Excel.Application();

                // for making Excel visible
                excel.Visible = false;
                excel.DisplayAlerts = false;

                // Creation a new Workbook
                excelworkBook = excel.Workbooks.Add(Type.Missing);

                // Workk sheet
                excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
                excelSheet.Name = worksheetName;


                excelSheet.Cells[1, 1] = ReporType;
                excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

                // loop through each row and add values to our sheet
                int rowcount = 2;

                //Add the column header
                for (int i = 1; i <= dataTable.Columns.Count; i++)
                {
                    excelSheet.Cells[2, i] = dataTable.Columns[i - 1].ColumnName;
                    excelSheet.Cells.Font.Color = System.Drawing.Color.Black;
                }

                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;
                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {

                        excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();

                        //for alternate rows
                        if (rowcount > 3)
                        {
                            if (i == dataTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    excelCellrange = excelSheet.Range[excelSheet.Cells[rowcount, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
                                    FormattingExcelCells(excelCellrange, "#CCCCFF", System.Drawing.Color.Black, false);
                                }

                            }
                        }

                    }

                }

                // now we resize the columns
                //TODO Handle 0 column without error
                if (dataTable.Columns.Count > 0)
                {
                    excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
                    excelCellrange.EntireColumn.AutoFit();

                    Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                    border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    border.Weight = 2d;


                    excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[2, dataTable.Columns.Count]];
                    FormattingExcelCells(excelCellrange, "#000099", System.Drawing.Color.White, true);
                }

                //now save the workbook and exit Excel


                excelworkBook.SaveAs(saveAsLocation); ;
                excelworkBook.Close();
                excel.Quit();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                excelSheet = null;
                excelCellrange = null;
                excelworkBook = null;
            }

        }

        /// <summary>
        /// Write the ILIST of datatable to excel
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="worksheetName"></param>
        /// <param name="saveAsLocation"></param>
        /// <param name="ReporType"></param>
        /// <returns></returns>
        public bool WriteDataTableToExcel(string saveAsLocation, List<ExcelWorksheet> worksheets)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelworkBook;

            try
            {
                // Start Excel and get Application object.
                excel = new Microsoft.Office.Interop.Excel.Application();

                // for making Excel visible
                excel.Visible = false;
                excel.DisplayAlerts = false;

                // Creation a new Workbook
                excelworkBook = excel.Workbooks.Add(Type.Missing);

                // Workk sheet
                foreach (var worksheet in worksheets)
                {
                    Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelworkBook.Sheets.Add(Type.Missing, Type.Missing, 1, Type.Missing);
                    Microsoft.Office.Interop.Excel.Range excelCellrange;
                    excelSheet.Name = worksheet.worksheetName;


                    excelSheet.Cells[1, 1] = worksheet.reporType;
                    excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

                    // loop through each row and add values to our sheet
                    int rowcount = 2;

                    //Add the column header
                    for (int i = 1; i <= worksheet.dataTable.Columns.Count; i++)
                    {
                        excelSheet.Cells[2, i] = worksheet.dataTable.Columns[i - 1].ColumnName;
                        excelSheet.Cells.Font.Color = System.Drawing.Color.Black;
                    }

                    foreach (DataRow datarow in worksheet.dataTable.Rows)
                    {
                        rowcount += 1;
                        for (int i = 1; i <= worksheet.dataTable.Columns.Count; i++)
                        {

                            excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();

                            //for alternate rows
                            if (rowcount > 3)
                            {
                                if (i == worksheet.dataTable.Columns.Count)
                                {
                                    if (rowcount % 2 == 0)
                                    {
                                        excelCellrange = excelSheet.Range[excelSheet.Cells[rowcount, 1], excelSheet.Cells[rowcount, worksheet.dataTable.Columns.Count]];
                                        FormattingExcelCells(excelCellrange, "#CCCCFF", System.Drawing.Color.Black, false);
                                    }

                                }
                            }

                        }

                    }

                    if (worksheet.dataTable.Columns.Count > 0)
                    {
                        excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[rowcount, worksheet.dataTable.Columns.Count]];
                        excelCellrange.EntireColumn.AutoFit();

                        Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                        border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        border.Weight = 2d;


                        excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[2, worksheet.dataTable.Columns.Count]];
                        FormattingExcelCells(excelCellrange, "#000099", System.Drawing.Color.White, true);
                    }

                }



                //now save the workbook and exit Excel

                excelworkBook.SaveAs(saveAsLocation); ;
                excelworkBook.Close();
                excel.Quit();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while saving Excel file : ", ex);
            }
            finally
            {
                excelSheet = null;
                excelCellrange = null;
                excelworkBook = null;
            }

        }

        /// <summary>
        /// FUNCTION FOR FORMATTING EXCEL CELLS
        /// </summary>
        /// <param name="range"></param>
        /// <param name="HTMLcolorCode"></param>
        /// <param name="fontColor"></param>
        /// <param name="IsFontbool"></param>
        public void FormattingExcelCells(Microsoft.Office.Interop.Excel.Range range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        {
            range.Interior.Color = System.Drawing.ColorTranslator.FromHtml(HTMLcolorCode);
            range.Font.Color = System.Drawing.ColorTranslator.ToOle(fontColor);
            if (IsFontbool == true)
            {
                range.Font.Bold = IsFontbool;
            }
        }

    }

}