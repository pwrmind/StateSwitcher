namespace StateSwitcher.Runtime;

/// <summary>
/// Represents the possible causes that can trigger state transitions in a Cell.
/// </summary>
public enum CellStateTransitionCause 
{ 
    /// <summary>The cell becomes empty</summary>
    Empty, 
    /// <summary>The cell becomes filled</summary>
    Filled, 
    /// <summary>The cell is booked</summary>
    Booked, 
    /// <summary>The cell becomes broken</summary>
    Broken, 
    /// <summary>The cell is hacked</summary>
    Hacked, 
    /// <summary>The cell becomes polluted</summary>
    Polluted, 
    /// <summary>The cell is fixed</summary>
    Fixed 
}
