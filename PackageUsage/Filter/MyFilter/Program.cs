// See https://aka.ms/new-console-template for more information

using DemoData;
using MyFilter;

const bool needReverse = true;
var results = StaticData.ListStrings.If(needReverse, s => s.Reverse());
foreach (var result in results)
{
    Console.WriteLine(result);
}