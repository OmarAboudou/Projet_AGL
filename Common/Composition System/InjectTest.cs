using Common.Composition_System.Inject_Attributes;
using Godot;

namespace Common.Composition_System;

public partial class InjectTest : Node
{
    [Export, InjectChild] private CsgBox3D _child;
    [Export, InjectSibling] private Control _sibling;
    [Export, InjectParent] private Node _parent;
    [Export, InjectAncestor] private Node2D _ancestor;
    [Export, InjectParentSibling] private CanvasModulate _parentSibling;
}