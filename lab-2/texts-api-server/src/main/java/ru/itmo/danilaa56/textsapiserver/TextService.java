package ru.itmo.danilaa56.textsapiserver;

import org.springframework.stereotype.Service;
import ru.itmo.danilaa56.textsapiserver.entities.Person;
import ru.itmo.danilaa56.textsapiserver.entities.Text;

import java.util.Collection;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.UUID;

@Service
public class TextService {

    private final Map<UUID, Text> texts = new LinkedHashMap<>();

    public TextService() {
        Person author = new Person("Ivan", "Whatshisname");
        createText("Hello world", author);
        createText("Hello world #2 Come back", author);
        createText("Hello world #3 Forever", author);
    }

    public Collection<Text> getTexts() {
        return texts.values();
    }

    public Text createText(String content, Person author) {
        var uuid = UUID.randomUUID();
        var now = new Date();
        var text = new Text(uuid, content, author, now, now);
        texts.put(uuid, text);
        return text;
    }

    public Text updateText(UUID id, String content) {
        var text = texts.get(id);
        var newText = new Text(id, content, text.author(), text.creationDate(), new Date());
        texts.replace(id, newText);
        return newText;
    }

    public void deleteText(UUID id) {
        texts.remove(id);
    }
}
