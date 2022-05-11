import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;
import com.sun.net.httpserver.HttpServer;
import com.sun.net.httpserver.spi.HttpServerProvider;

import java.io.IOException;
import java.net.InetSocketAddress;

public class Main {

    public static void main(String[] args) throws IOException {
        Server


        HttpServer server = HttpServerProvider.provider().createHttpServer(new InetSocketAddress(8080), 100);
        var context = server.createContext("/");
        context.setHandler(new HttpHandler() {
            @Override
            public void handle(HttpExchange exchange) throws IOException {
                System.out.println(exchange);
                System.out.println(exchange);
            }
        });
        server.start();
    }

//    private static class TestServlet implements Servlet

}
