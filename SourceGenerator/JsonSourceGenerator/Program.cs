using System.Text.Json;
using JsonSourceGenerator;

var ken = new Person { Name = "ken", Age = 29 };

var options = new JsonSerializerOptions { WriteIndented = true };

var text = JsonSerializer.Serialize(ken, options);
Console.WriteLine(text);
var k = JsonSerializer.Deserialize<Person>(text);
Console.WriteLine(k);


var text2 = JsonSerializer.Serialize(ken, PersonJsonContext.Default.Person);
Console.WriteLine(text2);
var k2 = JsonSerializer.Deserialize(text2, PersonJsonContext.Default.Person);
Console.WriteLine(k2);