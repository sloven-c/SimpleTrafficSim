namespace PrometSim.Structures;

public static class DataStructures {
    /// <summary>
    ///     Stores all possible area locations
    /// </summary>
    public enum AreaLocation {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    
    public enum RoadColor {
        Default,
        Selected
    }

    public struct CarSlot {
        public CarSlot() {
        }

        public int Priority { get; set; } = 0;
        public bool Occupied { get; set; } = false;
    }
}