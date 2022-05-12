package ru.itmo.danilaa56.textsapiserver.controller;

import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import ru.itmo.danilaa56.textsapiserver.TextService;
import ru.itmo.danilaa56.textsapiserver.entities.Text;
import ru.itmo.danilaa56.textsapiserver.utils.TextsException;

import java.util.Collection;
import java.util.UUID;

@RestController
@RequestMapping("text")
public class TextController {

    private final TextService textService;

    public TextController(TextService textService) {
        this.textService = textService;
    }

    @GetMapping
    public Collection<Text> list() {
        return textService.getTexts();
    }

    @PostMapping
    public Text add(@RequestBody Text text) {
        if (text.content() == null || text.author() == null)
            throw new TextsException("Content and author must be defined");
        return textService.createText(text.content(), text.author());
    }

    @PutMapping("{id}")
    public Text update(@PathVariable UUID id, @RequestBody Text text) {
        if (text.content() == null)
            throw new TextsException("Id and content must be defined");
        return textService.updateText(text.id(), text.content());
    }

    @DeleteMapping("{id}")
    public void delete(@PathVariable UUID id) {
        textService.deleteText(id);
    }
}
