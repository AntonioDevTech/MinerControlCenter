using Microsoft.Data.SqlClient;

// CONNECTION STRING
string connectionString = "Server=.;Database=MinerInventory;Trusted_Connection=True;TrustServerCertificate=True;";

while (true)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("=================================================================================");
    Console.WriteLine("   UNIFIED MINER MANAGER v3.0 (Bitmain/WhatsMiner Integration)   ");
    Console.WriteLine("=================================================================================");
    Console.ResetColor();
    Console.WriteLine("1. Monitor All Miners (Dashboard)");
    Console.WriteLine("2. Add New Device");
    Console.WriteLine("3. Remove Device");
    Console.WriteLine("4. Exit");
    Console.Write("\nSelect Option: ");

    string input = Console.ReadLine();

    if (input == "4") break;

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        try
        {
            conn.Open();

            if (input == "1") // === DASHBOARD VIEW ===
            {
                Console.WriteLine("\nFetching Telemetry...");
                string sql = "SELECT * FROM Miners";
                SqlCommand cmd = new SqlCommand(sql, conn);

                // Print Table Headers
                Console.WriteLine("\nID  | IP ADDRESS    | MODEL           | HASH    | PSU MODEL    | FIRMWARE  | STATUS");
                Console.WriteLine("---------------------------------------------------------------------------------------");

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // We read the data using the EXACT column names from your SQL Table
                        string id = reader["MinerID"].ToString();
                        string ip = reader["IPAddress"].ToString();
                        string name = reader["MinerName"].ToString();
                        string hash = reader["Hashrate"].ToString();
                        string psu = reader["PowerSupply"].ToString();
                        string firm = reader["FirmwareVer"].ToString();
                        string status = reader["Status"].ToString();

                        // This formatting aligns the columns perfectly
                        Console.WriteLine($"{id,-3} | {ip,-13} | {name,-15} | {hash,-7} | {psu,-12} | {firm,-9} | {status}");
                    }
                }
                Console.WriteLine("---------------------------------------------------------------------------------------");
            }
            else if (input == "2") // === ADD COMPLEX MINER ===
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n--- NEW DEVICE REGISTRATION ---");
                Console.ResetColor();

                Console.Write("Model Name (e.g. Antminer S19): ");
                string name = Console.ReadLine();

                Console.Write("IP Address (e.g. 192.168.1.50): ");
                string ip = Console.ReadLine();

                Console.Write("Hashrate (TH/s): ");
                string hash = Console.ReadLine();

                Console.Write("Power Supply Model: ");
                string psu = Console.ReadLine();

                Console.Write("Firmware Version: ");
                string firm = Console.ReadLine();

                Console.Write("Main Pool URL: ");
                string pool = Console.ReadLine();

                string sql = $"INSERT INTO Miners (MinerName, IPAddress, Hashrate, PowerSupply, FirmwareVer, PoolMain, Status) " +
                             $"VALUES ('{name}', '{ip}', {hash}, '{psu}', '{firm}', '{pool}', 'Online')";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ SUCCESS: {name} registered.");
                Console.ResetColor();
            }
            else if (input == "3") // === DELETE ===
            {
                Console.Write("\nEnter Miner ID to remove: ");
                string id = Console.ReadLine();
                string sql = $"DELETE FROM Miners WHERE MinerID = {id}";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Device Removed.");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: " + ex.Message);
            Console.ResetColor();
        }
    }
    Console.WriteLine("\nPress Enter to continue...");
    Console.ReadLine();
}