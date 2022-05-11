package ru.itmo.danilaa56.textsapiserver.entities;

public record Person(String name, String surname) {
    @Override
    public String name() {
        return name;
    }

    @Override
    public String surname() {
        return surname;
    }
}
