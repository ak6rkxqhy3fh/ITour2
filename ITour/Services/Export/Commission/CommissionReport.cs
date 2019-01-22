using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace ITour.Services.Export.Commission
{
    public class CommissionReport
    {
        private List<Models.Commission> Commission { get; set; }

        public byte[] CreateReportAsBytes(List<Models.Commission> commission)
        {
            Commission = commission;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet($"{DateTime.Today.ToShortDateString()}");

                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Заказ");
                row.CreateCell(1).SetCellValue("Агентство");
                row.CreateCell(2).SetCellValue("Менеджер");
                row.CreateCell(3).SetCellValue("Заказчик");
                row.CreateCell(4).SetCellValue("Туроператор");
                row.CreateCell(5).SetCellValue("Партнер");
                row.CreateCell(6).SetCellValue("Стоимость Заказа");
                row.CreateCell(7).SetCellValue("Входящие платежи");
                row.CreateCell(8).SetCellValue("Долг Заказчика");
                row.CreateCell(9).SetCellValue("Коммисия банка");
                row.CreateCell(10).SetCellValue("Исходящие платежи");
                row.CreateCell(11).SetCellValue("Комиссия");
                row.CreateCell(12).SetCellValue("Дата комиссии");

                for (int i = 0; i < Commission.Count; i++)
                {
                    row = excelSheet.CreateRow(i + 1);

                    row.CreateCell(0).SetCellValue((int)Commission[i].OrderNumber);
                    row.CreateCell(1).SetCellValue(Commission[i].AgencyCompanyName);
                    row.CreateCell(2).SetCellValue(Commission[i].ManagerName);
                    if (!string.IsNullOrEmpty(Commission[i].CustomerCompanyName))
                    {
                        row.CreateCell(3).SetCellValue($"{Commission[i].CustomerCompanyName} ({Commission[i].CustomerName})");
                    }
                    else
                    {
                        row.CreateCell(3).SetCellValue($"{Commission[i].CustomerName}");
                    }
                    row.CreateCell(4).SetCellValue(Commission[i].Touroperators);
                    row.CreateCell(5).SetCellValue(Commission[i].Partners);
                    if (Commission[i].OrderCost.HasValue)
                        row.CreateCell(6).SetCellValue((double)Commission[i].OrderCost);
                    if (Commission[i].IncomingPaymentsTotal.HasValue)
                        row.CreateCell(7).SetCellValue((double)Commission[i].IncomingPaymentsTotal);
                    if (Commission[i].CustomerDebt.HasValue)
                        row.CreateCell(8).SetCellValue((double)Commission[i].CustomerDebt);
                    if (Commission[i].BankCommissionTotal.HasValue)
                        row.CreateCell(9).SetCellValue((double)Commission[i].BankCommissionTotal);
                    if (Commission[i].OutgoingPaymentsTotal.HasValue)
                        row.CreateCell(10).SetCellValue((double)Commission[i].OutgoingPaymentsTotal);
                    if (Commission[i].OrderCommission.HasValue)
                        row.CreateCell(11).SetCellValue((double)Commission[i].OrderCommission);
                    if (Commission[i].OrderCommissionDate.HasValue)
                        row.CreateCell(12).SetCellValue(((DateTime)Commission[i].OrderCommissionDate).ToShortDateString());
                }

                workbook.Write(memoryStream);

                memoryStream.Flush();
                memoryStream.Close();

                return memoryStream.ToArray();
            }
        }
    }
}
