# Flowsy Types

## PrefixedGuid

The **PrefixedGuid** type handles values based on System.Guid with a custom prefix to identify the entity they represent:
* usr_58715d38-8b43-4fd0-8396-dae1bc97e471 (user)
* cus_688292c9-a6a9-46b3-9ae7-588cbf80d24a (customer)
* inv_ad32fd1b-cd1b-4366-91a9-c555b4de92a8 (invoice)
* ...

It's easy to use and allows for:
* Implicit conversion to and from the standard String type.
* Comparison using the == and != operators and the CompareTo and Equals methods.
* Parsing using the Parse and TryParse methods.

```csharp
var userId = PrefixedGuid.New("usr");

string str = userId;

PrefixedGuid pid = str;

var anotherUserId = PrefixedGuid.Parse("usr_58715d38-8b43-4fd0-8396-dae1bc97e4712");

if (PrefixedGuid.TryParse("usr_58715d38-8b43-4fd0-8396-dae1bc97e4712", out var myId))
{
    // Do something with myId
}
```