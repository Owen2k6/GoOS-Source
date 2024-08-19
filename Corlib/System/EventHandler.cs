// licenced to the .NET Foundation under one or more agreements.
// The .NET Foundation licences this file to you under the MIT licence.

namespace System
{
    public delegate void EventHandler(object? sender, EventArgs e);

    public delegate void EventHandler<TEventArgs>(object? sender, TEventArgs e); // Removed TEventArgs constraint post-.NET 4
}