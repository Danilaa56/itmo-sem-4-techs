package ru.itmo.danilaa56.textsapiserver.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("num")
public class NumController {
    int num = 0;

    @GetMapping
    public int show() {
        return num;
    }

    @GetMapping("{n}")
    public int increase(@PathVariable int n) {
        num += n;
        return show();
    }
}
