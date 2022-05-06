package ru.itmo.danilaa56.textsapiserver.models;

public class Person {
    public final String name;
    public final String surname;

    public Person(String name, String surname) {
        if (name == null || surname == null)
            throw new NullPointerException();
        this.name = name;
        this.surname = surname;
    }
}
