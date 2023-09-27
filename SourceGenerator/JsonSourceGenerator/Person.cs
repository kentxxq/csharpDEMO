namespace JsonSourceGenerator;

public class Person
{
    public string Name { get; set; } = default!;
    public int Age { get; set; }

    public override string ToString()
    {
        return $"Name:{Name},Age:{Age}";
    }
}