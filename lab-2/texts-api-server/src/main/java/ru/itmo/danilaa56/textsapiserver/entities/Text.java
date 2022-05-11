package ru.itmo.danilaa56.textsapiserver.entities;

import java.util.Date;
import java.util.UUID;

public record Text(UUID id, String content, Person author,
                   Date creationDate, Date lastUpdateDate) {
    @Override
    public UUID id() {
        return id;
    }

    @Override
    public String content() {
        return content;
    }

    @Override
    public Person author() {
        return author;
    }

    @Override
    public Date creationDate() {
        return creationDate;
    }

    @Override
    public Date lastUpdateDate() {
        return lastUpdateDate;
    }
}
