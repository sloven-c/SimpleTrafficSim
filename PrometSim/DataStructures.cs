namespace PrometSim;

public static class DataStructures {
    /// <summary>
    /// Stores all possible area locations
    /// </summary>
    public enum AreaLocation {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public struct CarSlot {
        public int Priority { get; set; } = 0;
        public bool Occupied { get; set; } = false;

        public CarSlot() {
        }
    }
}