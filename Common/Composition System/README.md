# Composition System

## Injections

System that allows for automatically searching and injecting node(s) 
from the surrounding of the current node. 

The considered surrounding is determined by the attributes used 
on the field or property.

### Attributes

|Name|Usage|
|:--:|:--:|
|InjectSibling|Inject a node that is a sibling of the current node|
|InjectChild|Inject a node that is a child of the current node|
|InjectParent|Inject the parent of the current node|
|InjectAncestor|Inject a node that is an ancestor of the current node|
|InjectParentSibling|Inject a node that is a sibling of the parent of the current node|

### Collections

When using **InjectAttributes** on a field or a property of a type that implements
***ICollection<>***, the collection will be populated with every node that validate
the injection.

**The field or property must not be equal to null** but should be initialized to empty.

#### Exemple : 

```csharp
public partial class SomeNode : Node
{
    // ...
    
    // This List will be filled with every ancestor of the node
    // that is compatible with the Node2D type
    [InjectAncestor] private List<Node2D> _ancestors2D;

    // ...
}
```