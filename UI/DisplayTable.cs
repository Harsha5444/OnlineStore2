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
            Console.WriteLine(new string('-', 77));
            foreach (DataColumn column in dataTable.Columns)
            {
                Console.Write(column.ColumnName.PadRight(20));
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', 77));
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write(item.ToString().PadRight(20));
                }
                Console.WriteLine();
            }
            Console.WriteLine(new string('-', 77));
            Console.WriteLine();
        }
    }
}
