using Raylib_cs;

namespace SimpleTrafficSim;

class Program : GameData {
    private static void Main() {
        Raylib.InitWindow(Resolution.width, Resolution.height, "Simple Traffic Sim");

        var areas = new Areas(3);

        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.LightGray);
            
            areas.Draw();
            
            Raylib.EndDrawing();
        }
    }
}