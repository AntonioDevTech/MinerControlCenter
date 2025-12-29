using Microsoft.Data.SqlClient;

string connectionString = "Server=.;Database=MinerInventory;Trusted_Connection=True;TrustServerCertificate=True;";

while (true)
{
    // 1. Draw the Menu
    Console.Clear(); // Clears the screen to look clean
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("=================================");
    Console.WriteLine("   MINER CONTROL CENTER v1.0     ");
    Console.WriteLine("=================================");
    Console.ResetColor();
    Console.WriteLine("1. View Inventory");
    Console.WriteLine("2. Add New Miner");
    Console.WriteLine("3. Exit");
    Console.Write("\nSelect an option: ");

    string input = Console.ReadLine();

    // 2. Handle the User's Choice
    if (input == "3") break; // Exit the app

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();

        if (input == "1") // === VIEW INVENTORY ===
        {
            Console.WriteLine("\n--- CURRENT INVENTORY ---");
            string sql = "SELECT * FROM Miners";
            SqlCommand cmd = new SqlCommand(sql, conn);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["MinerID"]} | {reader["MinerName"]} | {reader["Hashrate"]} TH/s | {reader["Status"]}");
                }
            }
        }
        else if (input == "2") // === ADD NEW MINER ===
        {
            // Ask user for details
            Console.Write("\nEnter Miner Name: ");
            string newName = Console.ReadLine();

            Console.Write("Enter Hashrate: ");
            string newHash = Console.ReadLine();

            Console.Write("Enter Status (Online/Offline): ");
            string newStatus = Console.ReadLine();

            // Insert into Database
            string sql = $"INSERT INTO Miners (MinerName, Hashrate, Status) VALUES ('{newName}', {newHash}, '{newStatus}')";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery(); // "NonQuery" means we aren't reading data, we are changing it

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✅ SUCCESS: Miner saved to database!");
            Console.ResetColor();
        }
    }

    Console.WriteLine("\nPress Enter to return to menu...");
    Console.ReadLine();
}