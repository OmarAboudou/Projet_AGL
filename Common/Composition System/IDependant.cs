using System.Collections.Generic;
using Godot;

namespace Composition.Composition_System;

public interface IDependant
{
    void DependencyAvailable(Node available);
    void DependencyUnavailable(Node unavailable);
}