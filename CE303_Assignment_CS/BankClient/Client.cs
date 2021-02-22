using System;
using System.IO;
using System.Net.Sockets;

namespace BankClient {
    class Client : IDisposable {
        const int port = 8888;

        private readonly StreamReader reader;
        private readonly StreamWriter writer;
        public int UniqueId;

        public Client () {
            // Connecting to the server and creating objects for communications
            TcpClient tcpClient = new TcpClient ("localhost", port);
            NetworkStream stream = tcpClient.GetStream ();
            reader = new StreamReader (stream);
            writer = new StreamWriter (stream);

            // Parsing the response
            string line = reader.ReadLine ();
            if (line.Trim ().ToLower () != "success")
                throw new Exception (line);
        }

        public bool HasStock () {
            // Writing the command
            writer.WriteLine ("BALANCE " + UniqueId);
            writer.Flush ();

            // Reading the account balance
            return bool.Parse (reader.ReadLine ());
        }

        public void Transfer (int fromTrader, int toTrader) {
            // Writing the command
            writer.WriteLine ($"TRANSFER {fromTrader} {toTrader}");
            writer.Flush ();

            // Reading the response
            string line = reader.ReadLine ();
            if (line.Trim ().ToLower () != "success")
                throw new Exception (line);
        }

        public string GetInfo () {
            // Writing the command
            writer.WriteLine ("GET");
            writer.Flush ();

            // Reading the response
            return reader.ReadLine ();
        }

        public string GetIds () {
            // Writing the command
            writer.WriteLine ("TRADERS");
            writer.Flush ();
            string output = reader.ReadLine ();
            String temp = output;
            try {
                temp = output.Substring (1, output.Length - 2);
                int.Parse (temp);
            } catch { }

            // Reading the response
            return temp;
        }

        public int Id () {
            // Writing the command
            writer.WriteLine ("ID");
            writer.Flush ();
            UniqueId = int.Parse (reader.ReadLine ());

            // Reading the response
            return UniqueId;
        }

        public void exit () {
            // Writing the command
            writer.WriteLine ("EXIT");
        }

        public void Dispose () {
            reader.Close ();
            writer.Close ();
        }
    }
}