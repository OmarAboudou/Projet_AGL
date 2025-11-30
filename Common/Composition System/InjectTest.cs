using Composition.Composition_System.Inject_Child;
using Composition.Composition_System.Inject_Parent;
using Composition.Composition_System.Inject_Sibling;
using Godot;

namespace Composition.Composition_System;

public partial class InjectTest : Node
{
    [Export, InjectSibling] private Node _sibling;
    [Export, InjectParent] private Node _parent;
    [Export, InjectChild] private Node _child;
}