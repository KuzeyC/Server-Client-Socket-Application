package assignment;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;

public class ServerProgram {
    private final static int port = 8888;

    private static final Bank bank = new Bank();

    private static int uniqueID = 0;

    public static void main(String[] args) {
        ServerProgram server = new ServerProgram();
        server.RunServer();
    }

    private void RunServer(){
        ServerSocket serverSocket = null;
        try {
            serverSocket = new ServerSocket(port);
            System.out.println("Waiting for incoming connections...");
            while (true) {
                Socket socket = serverSocket.accept();
                socket.getInputStream();
                uniqueID++;
                new Thread(new ClientHandler(socket, bank, uniqueID)).start();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
