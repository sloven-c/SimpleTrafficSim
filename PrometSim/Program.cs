using Raylib_cs;

namespace PrometSim;

class Program : GameData {
    private static void Main() {
        Raylib.InitWindow(Size.width, Size.height, "Promet Sim");

        var areas = new Areas(3);
        var c = new Car(100, 100);

        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);
            
            areas.Draw();
            c.Draw();
            
            Raylib.EndDrawing();
        }
    }
}