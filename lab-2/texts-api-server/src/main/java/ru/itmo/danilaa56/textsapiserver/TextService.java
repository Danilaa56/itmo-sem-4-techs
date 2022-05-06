package ru.itmo.danilaa56.textsapiserver;

import org.springframework.stereotype.Service;
import ru.itmo.danilaa56.textsapiserver.models.TextModel;

import java.util.Collection;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.UUID;

@Service
public class TextsService {

    private Map<UUID, TextModel> texts = new LinkedHashMap<>();

    public Collection<TextModel> getTexts() {
        return texts.values();
    }

    public void addText(TextModel text) {
        texts.
    }
}
