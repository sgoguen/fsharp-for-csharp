<Query Kind="Program" />

//  If you're a C# developer who wants to learn more about 
//  functional programming

void Main() {
}

U[] MapArray<T,U>(T[] input, Func<T,U> transformItem) {
	var result = new U[input.Length];
	for (int i = 0; i < input.Length; i++) {
		result[i] = transformItem(input[i]);
	}
	return result;
}