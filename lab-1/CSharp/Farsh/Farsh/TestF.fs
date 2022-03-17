module Farsh.TestF

let numbers = seq [
    for i in 1 .. 5 -> i * i - 1
]

let (|>) x f = f x
let increase x = x + 1
let double x = x * 2

5 |> increase |> increase |> double |> double |> double 