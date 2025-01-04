using System;
using System.Data;

namespace UI
{
    class Display
    {
        public static void DisplayTable(DataTable dataTable)
        {
            Console.Clear();
            Console.WriteLine($"--- {dataTable.TableName.ToUpper()} ---");
            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.Ordinal < 1)
                    Console.Write($"{column.ColumnName}".PadRight(10));
                else
                    Console.Write($"{column.ColumnName}".PadRight(20));
            }
            Console.WriteLine();
            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.Ordinal < 1)
                    Console.Write("----------".PadRight(10));
                else
                    Console.Write("--------------------".PadRight(20));
            }
            Console.WriteLine();
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (i < 1)
                        Console.Write($"{row[i]}".PadRight(10));
                    else
                        Console.Write($"{row[i]}".PadRight(20));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
