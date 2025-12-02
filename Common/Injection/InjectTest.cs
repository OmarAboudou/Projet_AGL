using Common.Injection.Inject_Attributes;
using Godot;
using Godot.Collections;

namespace Common.Injection;

public sealed partial class InjectTest : Node
{
    [ExportGroup("Fields")]
    [Export, InjectChild] private CsgBox3D _childField;
    [Export, InjectSibling] private Control _siblingField;
    [Export, InjectParent] private Node _parentField;
    [Export, InjectAncestor] private Node2D _ancestorField;
    [Export, InjectParentSibling] private CanvasModulate _parentSiblingField;
    [Export, InjectChild, InjectAncestor, InjectParentSibling] private Array<CanvasItem> _childrenAndParentsField = new();
    [Export, InjectChild] private InjectableNode _injectTestChildField;
    [Export, InjectDescendant] private Array<Node> _injectDescendantsField = new();

    [ExportGroup("Properties")]
    [Export, InjectChild] private CsgBox3D ChildProperty { get; set; }
    [Export, InjectSibling] private Control SiblingProperty { get; set; }
    [Export, InjectParent] private Node ParentProperty { get; set; }
    [Export, InjectAncestor] private Node2D AncestorProperty { get; set; }
    [Export, InjectParentSibling] private CanvasModulate ParentSiblingProperty { get; set; }
    [Export, InjectChild, InjectAncestor, InjectParentSibling] private Array<CanvasItem> ChildrenAndParentsProperty { get; set; } = new();
    [Export, InjectChild] private InjectableNode InjectTestChildProperty { get; set; }
    [Export, InjectDescendant] private Array<Node> InjectDescendantsProperty { get; set; }= new();
    

}