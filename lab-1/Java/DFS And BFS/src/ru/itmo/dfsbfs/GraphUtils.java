package ru.itmo.dfsbfs;

import java.util.Comparator;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Objects;
import java.util.PriorityQueue;
import java.util.function.Consumer;

public class GraphUtils {
    public static <VertexT extends Vertex<VertexT>> void DFS(VertexT vertex, Consumer<VertexT> onVisit) {
        visit(new HashSet<>(), vertex, onVisit);
    }

    public static <VertexT extends Vertex<VertexT>> void BFS(VertexT vertex, Consumer<VertexT> onVisit) {
        HashMap<VertexT, Double> distances = new HashMap<>();
        distances.put(vertex, 0.0D);
        Objects.requireNonNull(distances);
        PriorityQueue<VertexT> vertices = new PriorityQueue<>(Comparator.comparingDouble(distances::get));
        vertices.add(vertex);

        while(!vertices.isEmpty()) {
            VertexT v = vertices.poll();
            onVisit.accept(v);
            double distance = distances.get(v);
            for(VertexT neighbour : v.getNeighbours()) {
                if (!distances.containsKey(neighbour)) {
                    distances.put(neighbour, distance + v.edgeLengthTo(neighbour));
                    vertices.add(neighbour);
                } else if (distance + 1.0D < (Double)distances.get(neighbour)) {
                    vertices.remove(neighbour);
                    distances.put(neighbour, distance + v.edgeLengthTo(neighbour));
                    vertices.add(neighbour);
                }
            }
        }
    }

    private static <VertexT extends Vertex<VertexT>> void visit(HashSet<VertexT> visited, VertexT vertex, Consumer<VertexT> onVisit) {
        onVisit.accept(vertex);
        visited.add(vertex);
        for (VertexT neighbour : vertex.getNeighbours()) {
            if (!visited.contains(neighbour)) {
                visit(visited, neighbour, onVisit);
            }
        }
    }
}
