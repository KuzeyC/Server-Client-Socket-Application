using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BankServer {
    class Program {
        private const int port = 8888;

        private static readonly Bank bank = new Bank ();

        private static int UniqueID = 0;

        static void Main (string[] args) {
            RunServer ();
        }

        private static void RunServer () {
            TcpListener listener = new TcpListener (IPAddress.Loopback, port);
            listener.Start ();
            Console.WriteLine ("Waiting for incoming connections...");
            while (true) {
                TcpClient tcpClient = listener.AcceptTcpClient ();
                new Thread (HandleIncomingConnection).Start (tcpClient);
                UniqueID++;
            }
        }

        private static void HandleIncomingConnection (object param) {
            Dictionary<int, Trader> traders = bank.GetCurrentTraders ();
            TcpClient tcpClient = (TcpClient) param;
            using (Stream stream = tcpClient.GetStream ()) {
                StreamWriter writer = new StreamWriter (stream);
                StreamReader reader = new StreamReader (stream);
                try {
                    Console.WriteLine ($"New connection; customer ID {UniqueID}");
                    bank.CreateTrader (UniqueID, traders.Count == 0);
                    Console.WriteLine ("Traders currently in the market:");
                    foreach (int i in traders.Keys) {
                        Console.WriteLine ("Trader " + i + ": " + traders[i].Stock);
                    }

                    writer.WriteLine ("SUCCESS");
                    writer.Flush ();

                    while (true) {
                        string line = reader.ReadLine ();
                        string[] substrings = line.Split (' ');
                        switch (substrings[0].ToLower ()) {
                            case "balance":
                                int account = int.Parse (substrings[1]);
                                writer.WriteLine (bank.HasTraderStock (account));
                                writer.Flush ();
                                break;

                            case "transfer":
                                int fromTrader = int.Parse (substrings[1]);
                                int toTrader = int.Parse (substrings[2]);
                                bank.Transfer (fromTrader, toTrader);
                                Console.WriteLine ("Trader: " + fromTrader + " gave stock to Trader: " + toTrader);
                                writer.WriteLine ("SUCCESS");
                                writer.Flush ();
                                break;

                            case "get":
                                String output = "Traders currently in the market: ";
                                foreach (int i in traders.Keys) {
                                    output += "Trader " + i + ": " + traders[i].Stock + ", ";
                                }
                                writer.WriteLine (output.Substring (0, output.Length - 2));
                                writer.Flush ();
                                break;

                            case "traders":
                                writer.WriteLine (String.Join (", ", bank.GetCurrentTraders ().Keys));
                                writer.Flush ();
                                break;

                            case "id":
                                writer.WriteLine (UniqueID);
                                writer.Flush ();
                                break;

                            case "exit":
                                tcpClient.Close ();
                                break;

                            default:
                                throw new Exception ($"Unknown command: {substrings[0]}.");
                        }
                    }
                } catch (Exception e) {
                    try {
                        writer.WriteLine ("ERROR " + e.Message);
                        writer.Flush ();
                        tcpClient.Close ();
                    } catch {
                        Console.WriteLine ("Failed to send error message.");
                    }
                } finally {
                    Trader leavingTrader = traders[UniqueID];
                    traders.Remove (UniqueID);
                    bank.RemoveFromTraders (UniqueID);
                    if (leavingTrader.Stock && traders.Count > 0) {
                        foreach (Trader t in traders.Values) {
                            t.Stock = true;
                            break;
                        }
                    }
                    Console.WriteLine ($"Trader {UniqueID} disconnected.");
                    Console.WriteLine ("Traders currently in the market:");
                    foreach (int i in traders.Keys) {
                        Console.WriteLine ("Trader " + i + ": " + traders[i].Stock);
                    }
                }
            }
        }
    }
}