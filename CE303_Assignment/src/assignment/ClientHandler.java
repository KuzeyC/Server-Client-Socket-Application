package assignment;

import java.io.PrintWriter;
import java.net.Socket;
import java.util.HashMap;
import java.util.Scanner;

public class ClientHandler implements Runnable {
    private final Socket socket;
    private Bank bank;
    private int uniqueID;
    PrintWriter writer;

    public ClientHandler(Socket socket, Bank bank, int uniqueID) {
        this.socket = socket;
        this.bank = bank;
        this.uniqueID = uniqueID;
    }

    @Override
    public void run() {
        HashMap<Integer, Trader> traders = bank.getCurrentTraders();
        try {
                Scanner scanner = new Scanner(socket.getInputStream());
                writer = new PrintWriter(socket.getOutputStream(), true);
            try {
                System.out.println("New connection. Trader ID " + uniqueID);
                bank.createTrader(uniqueID, traders.size() == 0);
                System.out.println("Traders currently in the market:");
                for (int i : traders.keySet()) {
                    System.out.println("Trader " + i + ": " + traders.get(i).hasStock());
                }

                writer.println("SUCCESS");
                while (true) {
                    String line = scanner.nextLine();
                    String[] substrings = line.split(" ");
                    switch (substrings[0].toLowerCase()) {
                        case "balance":
                            int account =  Integer.parseInt(substrings[1]);
                            writer.println(bank.hasTraderStock(account));
                            break;

                        case "transfer":
                            int fromTrader = Integer.parseInt(substrings[1]);
                            int toTrader = Integer.parseInt(substrings[2]);
                            bank.transfer(fromTrader, toTrader);
                            System.out.println("Trader: " + fromTrader + " gave stock to Trader: " + toTrader);
                            writer.println("SUCCESS");
                            break;

                        case "get":
                            System.out.println("Traders currently in the market:");
                            StringBuilder output = new StringBuilder();
                            for (int i : traders.keySet()) {
                                output.append("Trader ").append(i).append(": ").append(traders.get(i).hasStock()).append(", ");
                            }
                            writer.println(output.toString().substring(0, output.length() - 2));
                            break;

                        case "traders":
                            writer.println(bank.getCurrentTraders().keySet());
                            break;

                        case "id":
                            writer.println(uniqueID);
                            break;

                        case "exit":
                            socket.close();
                            break;
                        default:
                            throw new Exception("Unknown command: " + substrings[0]);
                    }
                }
            } catch (Exception e) {
                writer.println("ERROR " + e.getMessage());
                socket.close();
            }
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            System.out.println("Trader " + uniqueID + " disconnected.");
            Trader leavingTrader = traders.get(uniqueID);
            traders.remove(uniqueID);
            bank.traders.remove(uniqueID);
            if (leavingTrader.hasStock() && traders.size() > 0) {
                traders.get(traders.keySet().stream().findFirst().get()).setStock(true);
            }
            System.out.println("Traders currently in the market:");
            for (int i : traders.keySet()) {
                System.out.println("Trader " + i + ": " + traders.get(i).hasStock());
            }
        }
    }
}
