package ru.itmo.dfsbfs;

import java.util.List;

public abstract class Vertex<T> {
    abstract List<T> getNeighbours();

    abstract double edgeLengthTo(T var1);
}
