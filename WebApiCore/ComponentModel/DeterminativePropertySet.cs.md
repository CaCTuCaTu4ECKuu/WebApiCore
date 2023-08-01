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

Этот класс идеально подойдет для принятия запросов клиента.

Если мы имеем модель
```
class MyModel : DeterminativePropertySet<MyModel>
{
    public object PropertyA
    {
        get => Get<object>(e => e.PropertyA);
        set => Set(value, e => e.PropertyA);
    }

    public object PropertyB
    {
        get => Get<object>(e => e.PropertyB);
        set => Set(value, e => e.PropertyB);
    }
}
```

При выполнении запроса
```
 post => { PropertyA = "xxx" }
```

Привязанная модель будет
```
{
    myModel.IsDefined(e => e.PropertyA); // true
    myModel.IsDefined(e => e.PropertyB); // false
}
```
