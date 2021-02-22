package assignment;

import java.io.*;
import java.net.Socket;
import java.util.Scanner;

public class Client implements AutoCloseable {
    final int port = 8888;

    final Scanner reader;
    final PrintWriter writer;
    int uniqueId = 0;

    public Client() throws Exception {
        // Connecting to the server and creating objects for communications
        Socket socket = new Socket("localhost", port);
        socket.getRemoteSocketAddress();
        reader = new Scanner(socket.getInputStream());

        // Automatically flushes the stream with every command
        writer = new PrintWriter(socket.getOutputStream(), true);

        // Parsing the response
        String line = reader.nextLine();
        if (line.trim().compareToIgnoreCase("success") != 0)
            throw new Exception(line);
    }

    public boolean hasStock() {
        // Writing the command
        writer.println("BALANCE " + uniqueId);

        // Reading the account balance
        String line = reader.nextLine();
        return Boolean.parseBoolean(line);
    }

    public void transfer(int fromAccount, int toAccount) throws Exception {
        // Writing the command
        writer.println("TRANSFER " + fromAccount + " " + toAccount);

        // Reading the response
        String line = reader.nextLine();
        System.out.println(line);

        if (line.trim().compareToIgnoreCase("success") != 0)
            throw new Exception(line);
    }

    public String getInfo() {
        // Writing the command
        writer.println("GET");

        // Reading the response
        return reader.nextLine();
    }

    public String getIds() {
        // Writing the command
        writer.println("TRADERS");
        String output = reader.nextLine();
        String temp = output;
        try {
            temp = output.substring(1, output.length() - 1);
            Integer.parseInt(temp);
        } catch (Exception ignored) {}

        // Reading the response
        return temp;
    }

    public int id(){
        // Writing the command
        writer.println("ID");
        uniqueId = Integer.parseInt(reader.nextLine());

        // Reading the response
        return uniqueId;
    }

    public void exit(){
        // Writing the command
        writer.println("EXIT");
    }

    @Override
    public void close(){
        reader.close();
        writer.close();
    }
}
