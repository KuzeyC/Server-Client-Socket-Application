using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BankClient {
    class Program {
        static void Main (string[] args) {
            try {
                using (Client client = new Client ()) {
                    client.UniqueId = client.Id ();
                    Console.WriteLine ("Logged in successfully.");
                    Console.WriteLine ("You are Trader ID " + client.UniqueId);

                    while (true) {
                        Console.WriteLine ("Your Stock: " + client.HasStock ());

                        Console.WriteLine ("Enter the Trader ID to transfer to or -1 to print the Traders in the market:");
                        string toTrader = Console.ReadLine ();

                        try {
                            if (toTrader == "exit") {
                                Console.WriteLine ("Exiting the market.");
                                client.exit ();
                                break;
                            }
                            int toTraderInt = int.Parse (toTrader);
                            if (toTraderInt < 0) {
                                Console.WriteLine ("Traders in the market:");
                                Console.WriteLine (client.GetInfo ());
                            } else if (!checkTraders (new List<string> (client.GetIds ().Split (", ")), toTraderInt)) {
                                Console.WriteLine ("Trader not in the market.");
                            } else if (!client.HasStock ()) {
                                Console.WriteLine ("You do not own the stock. Unable to transfer.");
                            } else if (checkTraders (new List<string> (client.GetIds ().Split (", ")), toTraderInt)) {
                                client.Transfer (client.UniqueId, toTraderInt);
                                Console.WriteLine ("You gave the stock to Trader " + toTrader);
                            }
                        } catch {
                            Console.WriteLine ("Unkown command.");
                        }
                    }
                }
            } catch (Exception e) {
                Console.WriteLine (e.Message);
            }
        }
        private static bool checkTraders (List<string> l, int toTraderInt) {
            foreach (string s in l) {
                Console.WriteLine (s);
                if (toTraderInt == int.Parse (s)) {
                    return true;
                }
            }
            return false;
        }
    }
}