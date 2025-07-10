using Raylib_cs;

namespace SimpleTrafficSim;

class Program {
    static void Main(string[] args) {
        Raylib.InitWindow(800, 600, "Hello World");

        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            
            Raylib.ClearBackground(Color.Gray);
            
            Raylib.DrawText("Hello World", 12, 12, 20, Color.Red);
            
            Raylib.EndDrawing();
        }
    }
}