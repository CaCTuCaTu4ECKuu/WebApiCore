example class:
```
public class ExampleClass : DeterminativePropertySet<ExampleClass>
{
    public object Property
    {
        get => Get<object>(e => e.Property);
        set => Set(value, e => e.Property);
    }
}
```

example use:
```
{
    var ec = new ExampleClass();
    ec.IsDefined(e => e.Property); // return false
    ec.Property = 123;
    ec.IsDefined(e => e.Property); // return true
}
```