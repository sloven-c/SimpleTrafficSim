using Raylib_cs;
using Color = Raylib_cs.Color;

namespace PrometSim;

internal class Program {
    private static void Main() {
        Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint | ConfigFlags.ResizableWindow);
        Raylib.InitWindow(GameData.Size.width, GameData.Size.height, "Promet Sim");
        Raylib.MaximizeWindow();

        var areas = new Areas(3);
        var roads = new List<Road>();

        while (!Raylib.WindowShouldClose()) {
            WindowSizeChanged();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            InputHandler(roads);

            areas.Draw();

            foreach (var road in roads) road.Draw();

            if (Raylib.IsMouseButtonPressed(MouseButton.Left)) {
                var pos = Raylib.GetMousePosition();

                if (roads.Count == 0 || roads[^1].Confirmed())
                    // add new road if there are no roads or previous one is fully built
                    roads.Add(new Road(pos));
                else if (!roads[^1].Confirmed())
                    // if previous road is not build it, confirm the last coordinate
                    roads[^1].ConfirmRoad(pos);
            }
            else if (Raylib.IsMouseButtonPressed(MouseButton.Right) && roads.Count >= 1 && !roads[^1].Confirmed()) {
                // remove the last road that is being built
                roads.RemoveAt(roads.Count - 1);
            }

            Raylib.EndDrawing();
        }
    }

    private static void WindowSizeChanged() {
        var currentWidth = Raylib.GetScreenWidth();
        var currentHeight = Raylib.GetScreenHeight();

        if (currentWidth != GameData.Size.width || currentHeight != GameData.Size.height)
            GameData.Size = (currentWidth, currentHeight);
    }

    private static void InputHandler(List<Road> roads) {
        if (Raylib.IsKeyDown(KeyboardKey.C)) roads.Clear();
    }
}