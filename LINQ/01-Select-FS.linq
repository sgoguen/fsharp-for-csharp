<Query Kind="FSharpProgram" />

//  Now let's implement the same function in F#

let MapArray(input, transformItem) =
    [| for item in input -> transformItem(item) |]
    
let doubleIt(x) = x * x

MapArray([| 1;3;5;6;7|], doubleIt).Dump()

//  That's it for now..  Until next time.