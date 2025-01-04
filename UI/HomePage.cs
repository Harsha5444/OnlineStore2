using DataPassingBLL;
using System;
using System.Data;
using System.Linq;

namespace UI
{
    class HomePage
    {
        public static void Menu(DataSet ds, DataBLL bll)
        {
            while (true)
            {
                Console.WriteLine("********** Welcome to the Home Page **********");
            start:
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Add to Cart");
                Console.WriteLine("3. View Cart");
                Console.WriteLine("4. Checkout");
                Console.WriteLine("5. Logout");
                Console.Write("Please select an option: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewProducts(ds);
                        goto start;
                    case "2":
                        AddToCart(ds);
                        break;
                    case "3":
                        ViewCart(ds);
                        goto start;
                    case "4":
                        Checkout(ds, bll);
                        break;
                    case "5":
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        break;
                }
            }
        }
        public static void ViewProducts(DataSet ds)
        {
            Display.DisplayTable(ds.Tables["products"]);
        }
        public static void AddToCart(DataSet ds)
        {
            Console.Clear();
            Console.WriteLine("********** Add to Cart **********");
            Display.DisplayTable(ds.Tables["products"]);
            Console.Write("Enter the Product ID to add to the cart: ");
            int productId = Convert.ToInt32(Console.ReadLine());
            DataRow productRow = ds.Tables["products"]
                               .AsEnumerable()
                               .FirstOrDefault(row => row.Field<int>("ProductId") == productId);
            int availableQuantity = (int)productRow["QuantityAvailable"];
            Console.Write($"Enter the quantity of {productRow["ProductName"]}: ");
            int quantity;
            if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity. Please try again.");
                return;
            }
            if (quantity > availableQuantity)
            {
                Console.WriteLine("Insufficient stock available. Please try again.");
                return;
            }
            DataTable cartTable = ds.Tables["cart"];
            DataRow cartRows = ds.Tables["cart"]
                                .AsEnumerable()
                                .FirstOrDefault(row => row.Field<int>("ProductId") == productId);
            if (cartRows != null)
            {
                Console.WriteLine($"Product '{productRow["ProductName"]}' is already in your cart.");
                Console.Write("Do you want to modify the quantity? (yes/no): ");
                string modifyChoice = Console.ReadLine()?.ToLower();
                if (modifyChoice == "yes")
                {
                    Console.Write($"Enter the new quantity for {productRow["ProductName"]}: ");
                    int newQuantity;
                    if (!int.TryParse(Console.ReadLine(), out newQuantity) || newQuantity <= 0)
                    {
                        Console.WriteLine("Invalid quantity. Please try again.");
                        return;
                    }
                    if (newQuantity > availableQuantity + (int)cartRows["Quantity"])
                    {
                        Console.WriteLine("Insufficient stock available for the updated quantity. Please try again.");
                        return;
                    }
                    int oldCartQuantity = (int)cartRows["Quantity"];
                    cartRows["Quantity"] = newQuantity;
                    cartRows["FinalPrice"] = newQuantity * (int)productRow["Price"];
                    productRow["QuantityAvailable"] = availableQuantity + oldCartQuantity - newQuantity;
                    Console.WriteLine($"Updated the cart with new quantity: {newQuantity} for {productRow["ProductName"]}.");
                }
                else
                {
                    Console.WriteLine("No changes made to the cart.");
                }
            }
            else
            {
                DataRow newCartRow = cartTable.NewRow();
                newCartRow["ProductId"] = productRow["ProductId"];
                newCartRow["Username"] = Session.UserName;
                newCartRow["Quantity"] = quantity;
                newCartRow["FinalPrice"] = quantity * (int)productRow["Price"];
                cartTable.Rows.Add(newCartRow);
                productRow["QuantityAvailable"] = availableQuantity - quantity;
                Console.WriteLine($"Successfully added {quantity} of {productRow["ProductName"]} to the cart.");
            }
            Console.WriteLine("\nPress any key to return to the menu.");
            Console.ReadKey();
        }
        public static void ViewCart(DataSet ds)
        {
            Console.Clear();
            Console.WriteLine("********** Your Cart **********");
            DataTable cartTable = ds.Tables["cart"];
            DataTable productsTable = ds.Tables["products"];
            if (cartTable.Rows.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");
                Console.WriteLine("\nPress any key to return to the menu.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Product Name".PadRight(30) + "Quantity".PadRight(15) + "Final Price");
            Console.WriteLine(new string('-', 60));
            foreach (DataRow cartRow in cartTable.Rows)
            {
                int productId = (int)cartRow["ProductId"];
                DataRow productRow = productsTable
                                    .AsEnumerable()
                                    .FirstOrDefault(row => row.Field<int>("ProductId") == productId);
                if (productRow != null)
                {
                    string productName = productRow["ProductName"].ToString();
                    int quantity = (int)cartRow["Quantity"];
                    int finalPrice = (int)cartRow["FinalPrice"];
                    Console.WriteLine(productName.PadRight(30) +
                                      quantity.ToString().PadRight(15) +
                                      finalPrice.ToString());
                }
            }
            Console.WriteLine("\nPress any key to return to the menu.");
            Console.ReadKey();
        }
        public static void Checkout(DataSet ds, DataBLL bll)
        {
            DataTable cartTable = ds.Tables["cart"];
            DataTable productsTable = ds.Tables["products"];
            if (cartTable.Rows.Count == 0)
            {
                Console.WriteLine("Your cart is empty. Cannot proceed with checkout.");
                Console.WriteLine("\nPress any key to return to the menu.");
                Console.ReadKey();
                return;
            }
            decimal totalCost = cartTable.AsEnumerable()
                                         .Sum(row => row.Field<int>("FinalPrice"));
            var orderDetailsList = cartTable.AsEnumerable()
                .Select(row =>
                {
                    var productId = row.Field<int>("ProductId");
                    var productRow = productsTable.AsEnumerable()
                        .FirstOrDefault(p => p.Field<int>("ProductId") == productId);
                    string productName = productRow["ProductName"].ToString();
                    int quantity = row.Field<int>("Quantity");
                    return $"{productName} x {quantity}";
                })
                .ToList();
            string orderDetails = string.Join(", ", orderDetailsList);
            DataTable ordersTable = ds.Tables["orders"];
            DataRow newOrderRow = ordersTable.NewRow();
            newOrderRow["Username"] = Session.UserName;
            newOrderRow["TotalCost"] = totalCost;
            newOrderRow["OrderDate"] = DateTime.Now;
            newOrderRow["OrderDetails"] = orderDetails;
            ordersTable.Rows.Add(newOrderRow);
            cartTable.Rows.Clear();
            bll.Changes(ds);
            Console.Clear();
            Console.WriteLine("********** Order Confirmation **********");
            Console.WriteLine($"Order placed successfully by {Session.UserName}.");
            Console.WriteLine($"Total Cost: {totalCost}");
            Console.WriteLine($"Order Date: {DateTime.Now}");
            Console.WriteLine($"Order Details: {orderDetails}");
            Console.WriteLine("\nPress any key to return to the homepage.");
            Console.ReadKey();
            Menu(ds, bll);
        }
    }
}
