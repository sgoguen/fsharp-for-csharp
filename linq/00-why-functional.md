# Why Functional?

Not long ago, a friend of mine asked me a question about a problem he was trying 
to solve on Codewars.  The problem he was working on was asking him to implement
a function that takes two lists as input and sees if every number in the first list
correlates with the square of that number in the second list.  Order doesn't matter.

If you got this as an input:

```
A: 1, 5, 8, 9
B: 81, 25, 1, 64
```

Your function should return true.

However, if you got this as an input:

```
A: 1, 5, 8, 9
B: 81, 25, 1, 64, 81
```

Your function should return false because every element doesn't correlate with one other element.

I asked him to send me some code and I received something that looked like this:

```csharp
bool comp(int[] a, int[] b) {
	//  Loop through each item in a
	for (int i = 0; i < a.Length; i++) {
		int x = a[i];

		// Let's see if we can find the square of the current item in b
		bool foundMatch = false;
		for (int j = 0; j < b.Length; j++) {
			int y = b[j];
			if (x * x != y) {
				foundMatch = true;
				break;
			}
		}
		//  If we couldn't, return false.  Not every item in list A had a 
		//  matching item in list B
		if (foundMatch == false) return false;
	}
	//  It seems every item has a match, let's return true
	return true;
}
```

The code seemed straight-forward enough, but it was clear to me why his function 
wasn't passing the tests.  It was merely checking to see if B contained a square 
of every item in A, not make sure there is a one-to-one correlation.

## Cheating with Libraries

Clearly, he was trying to solve the problem without using any libraries  (It wasn't a 
requirement).  However, by not using any libraries, his code was longer than it needed
to be.  All I could think was it could be solved with one line of F#:

```fsharp
let comp a b = List.sort [ for n in a -> n * n ] = List.sort b
```

To be fair, you could also solve it a few lines of C# like so:

```csharp
bool comp2(int[] a, int[] b) => 
	a.Select(n => n * n).OrderBy(n => n).SequenceEqual(b.OrderBy(n => n));
```

Of course, it's going to be easier if you can learn on a library that will 
transform, sort it and then compare sorted sequences for you than if you had 
to write that all yourself.  That's clear.

## If you can write the library, is it still cheating?

My friend felt like it was cheating because for him, the LINQ library is a black box.  
He's never implemented a library like that.  Few people have implemented libraries that 
use higher-ordered functions to transform data in a functional way.

## Higher-Ordered Functions can act like Macros

Before we got lambda functions in most languages, anytime we wanted to transform a list,
we've have to write something like this over and over again:

```csharp
int[] squaredArray = new int[a.Length];
for(var i = 0;i < a.Length;i++) {
   var n = a[i];
   squaredArray[i] = n * n;
}
```

Now our languages let us pass functions in as parameters.  Because I can pass a function
as a parameter, I can write *one* generic function that lets me turn 4-5 lines of imperative
code into something like this:

```csharp
var squaredArray = a.MapArray(n => n * n);
```

All I have do to is write one carefully thought-out generic extension method like so:

```csharp
public static U[] MapArray<T,U>(this T[] input, Func<T,U> transformItem) {
	var result = new U[input.Length];
	for (int i = 0; i < input.Length; i++) {
		result[i] = transformItem(input[i]);
	}
	return result;
}
```

What's great about these extension methods is I can easily chain them like so:

```csharp
//  First, add two, then square every item.
var transformed = a.MapArray(n => n + 2).MapArray(n => n * 2);
```

# It's even easier in F#

If the above example with generics was a little too complicated, check out this F# version:

```fsharp
let mapArray(array, transformItem) = [| for item in array -> transformItem(item) |]
```

If you really want to initialize and modify each element in an array, you can do write it like this:

```fsharp
let mapArray(array: 'a[]) (transformItem: 'a -> 'b) = 
    let result = Array.create<'b>(array.Length)(Unchecked.defaultof<'b>)
    for i in 0..array.Length do
        result.[i] <- transformItem(array.[i])
    result
```

But why would you?  Not only is this not idiomatic F# code.  It's slower too.
