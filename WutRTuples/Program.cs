// See https://aka.ms/new-console-template for more information
using WutRTuples;

var t = Worker.GetTuple();

var name = t.Item1;
var age = t.Item2;
Console.WriteLine($"My name is {name}");
Console.WriteLine($"I am {age} years old");