# Composition System

---
## Injections

System that allows for automatically searching and injecting node(s) 
from the surrounding of the current node. 

The considered surrounding is determined by the **InjectAttributes** used 
on the field or property.

When using multiple **InjectAttributes** on a field, the considered surrounding is
the union of every surroundings of every **InjectAttributes**.

### Attributes

|        Name         |                                      Usage                                       |
|:-------------------:|:--------------------------------------------------------------------------------:|
|    InjectSibling    |            Inject a node that has the same parent as the current one             |
|     InjectChild     |                 Inject a node that is a child of the current one                 |
|    InjectParent     |                      Inject the parent of the current node                       |
|   InjectAncestor    | Inject a node that is a parent, a grand-parent, or further from the current node |
| InjectParentSibling |       Inject a node that has the same parent as the current node's parent        |
|  InjectDescendant   |                    Inject a node that is a below in the tree                     |

### Collections

When using **InjectAttributes** on a field or a property of a type that implements
***ICollection<>***, the collection will be populated with every node that validate
the injection.

**The field or property must not be equal to null** but should be initialized to empty.

#### Exemple

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

---

## Overview

For an overview of what is possible using **InjectAttributes**, 
please consider checking the ***"InjectTest.tscn"*** scene 
and ***"InjectTest.cs"*** script.