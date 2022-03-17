package ru.itmo.dfsbfs;

import java.io.PrintStream;
import java.util.HashSet;
import java.util.List;
import java.util.Objects;
import java.util.Set;

public class Main {

    public static void main(String[] args) {
        Main.MyVertex[] vertices = new Main.MyVertex[5];

        for(int i = 0; i < 5; ++i) {
            vertices[i] = new Main.MyVertex(String.valueOf(i));
        }

        vertices[0].link(vertices[1]);
        vertices[1].link(vertices[2]);
        vertices[0].link(vertices[3]);
        vertices[3].link(vertices[4]);
        System.out.println("DFS: ");
        Main.MyVertex var10000 = vertices[0];
        PrintStream var10001 = System.out;
        Objects.requireNonNull(var10001);
        GraphUtils.DFS(var10000, var10001::println);
        System.out.println("BFS: ");
        var10000 = vertices[0];
        var10001 = System.out;
        Objects.requireNonNull(var10001);
        GraphUtils.BFS(var10000, var10001::println);
    }

    private static class MyVertex extends Vertex<Main.MyVertex> {
        private final String label;
        private final Set<Main.MyVertex> neighbours = new HashSet();

        public MyVertex(String label) {
            this.label = label;
        }

        public void link(Main.MyVertex other) {
            this.neighbours.add(other);
            other.neighbours.add(this);
        }

        @Override
        List<MyVertex> getNeighbours() {
            return this.neighbours.stream().toList();
        }

        @Override
        double edgeLengthTo(Main.MyVertex other) {
            return 1.0D;
        }

        @Override
        public String toString() {
            return this.label;
        }
    }
}
