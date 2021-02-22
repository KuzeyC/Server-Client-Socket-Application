package assignment;

import java.util.*;

public class ClientProgram {
    public static void main(String[] args) {
        try (Client client = new Client()) {
            try {
                client.uniqueId = client.id();
                System.out.println("Logged in successfully.");
                System.out.println("You are Trader ID " + client.uniqueId);

                while (true) {
                    System.out.println("------------------------------");
                    System.out.println("Your Stock: " + client.hasStock());

                    System.out.println("Enter the Trader ID to transfer to:");
                    System.out.println("-1 to print the Traders in the market:");
                    System.out.println("'exit' to exit the market.");

                    Scanner in = new Scanner(System.in);
                    String toTrader = in.nextLine();
                    try {
                        System.out.println(Arrays.asList(client.getIds().split(", ")));
                        if (toTrader.equals("exit")) {
                            System.out.println("Exiting the market.");
                            client.exit();
                            break;
                        }
                        int toTraderInt = Integer.parseInt(toTrader);
                        if (toTraderInt < 0) {
                            System.out.println("Traders in the market:");
                            System.out.println(client.getInfo());
                        } else if (!Arrays.asList(client.getIds().split(", ")).contains(toTrader)) {
                            System.out.println("Trader not in the market.");
                        } else if (!client.hasStock()) {
                            System.out.println("You do not own the stock. Unable to transfer.");
                        } else if (Arrays.asList(client.getIds().split(", ")).contains(toTrader)) {
                            client.transfer(client.uniqueId, toTraderInt);
                            System.out.println("You gave the stock to Trader " + toTrader);
                        }
                    } catch (Exception e) {
                        System.out.println("Unknown command.");
                    }
                }

            } catch (Exception e) {
                System.out.println(e.getMessage());
            }
        } catch (Exception e) {
            System.out.println(e.getMessage());
        }
    }
}
