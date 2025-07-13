using Raylib_cs;

namespace PrometSim;

internal class Program : GameData {
    private static void Main() {
        Raylib.InitWindow(Size.width, Size.height, "Promet Sim");

        var areas = new Areas(3);
        Road? road = null;

        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            areas.Draw();

            road?.Draw();

            if (Raylib.IsMouseButtonPressed(MouseButton.Left)) {
                var pos = Raylib.GetMousePosition();

                if (road == null)
                    road = new Road(pos);
                else if (!road.Confirmed()) road.ConfirmRoad(pos);
            }
            else if (Raylib.IsMouseButtonPressed(MouseButton.Right)) {
                road = null;
            }

            Raylib.EndDrawing();
        }
    }
}