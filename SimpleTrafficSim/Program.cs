using Raylib_cs;

namespace SimpleTrafficSim;

class Program : GameData {
    private static void Main() {
        Raylib.InitWindow(Size.width, Size.height, "Simple Traffic Sim");

        var areas = new Areas(3);
        var c = new Car(100, 100);

        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.LightGray);
            
            areas.Draw();
            c.Draw();
            
            Raylib.EndDrawing();
        }
    }
}