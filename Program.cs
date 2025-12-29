using Microsoft.Data.SqlClient;
using System.Net.NetworkInformation; // New: This allows the "Ping" feature

string connectionString = "Server=.;Database=MinerInventory;Trusted_Connection=True;TrustServerCertificate=True;";

while (true)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("=================================================================================");
    Console.WriteLine("   UNIFIED MINER MANAGER v4.0 (Live Status Tracking)   ");
    Console.WriteLine("=================================================================================");
    Console.ResetColor();
    Console.WriteLine("1. Monitor All Miners (Live Dashboard)");
    Console.WriteLine("2. Add New Device");
    Console.WriteLine("3. Remove Device");
    Console.WriteLine("4. Exit");
    Console.Write("\nSelect Option: ");

    string input = Console.ReadLine();
    if (input == "4") break;

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();
        if (input == "1")
        {
            Console.WriteLine("Scanning Network and Fetching Telemetry...");
            string sql = "SELECT * FROM Miners";
            SqlCommand cmd = new SqlCommand(sql, conn);

            Console.WriteLine("\nID  | IP ADDRESS    | MODEL           | HASH    | PSU MODEL    | FIRMWARE  | LIVE STATUS");
            Console.WriteLine("---------------------------------------------------------------------------------------");

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string ip = reader["IPAddress"].ToString();

                    // --- THE PING LOGIC ---
                    bool isAlive = false;
                    try
                    {
                        Ping myPing = new Ping();
                        PingReply reply = myPing.Send(ip, 1000); // Wait 1 second for a response
                        if (reply.Status == IPStatus.Success) { isAlive = true; }
                    }
                    catch { isAlive = false; }

                    Console.ForegroundColor = isAlive ? ConsoleColor.Green : ConsoleColor.Red;
                    string liveStatus = isAlive ? "ONLINE" : "OFFLINE";

                    Console.WriteLine($"{reader["MinerID"],-3} | {ip,-13} | {reader["MinerName"],-15} | {reader["Hashrate"],-7} | {reader["PowerSupply"],-12} | {reader["FirmwareVer"],-9} | {liveStatus}");
                    Console.ResetColor();
                }
            }
        }
        // ... (Keep your Add/Remove logic from the previous version below)
    }
    Console.WriteLine("\nPress Enter to continue...");
    Console.ReadLine();
}