using PrometSim.AreaStructures;
using PrometSim.RoadStructures;
using PrometSim.Structures;
using Raylib_cs;
using Color = Raylib_cs.Color;

namespace PrometSim;

internal class Program {
    private static void Main() {
        Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint | ConfigFlags.ResizableWindow);
        Raylib.InitWindow(GameData.Size.width, GameData.Size.height, "Promet Sim");

        Raylib.SetExitKey(KeyboardKey.Null);
        Raylib.MaximizeWindow();

        var areas = new Areas(3);
        var roadMan = new RoadManager();

        List<IDrawable> drawables = [areas, roadMan];

        while (!Raylib.WindowShouldClose()) {
            WindowSizeChanged();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            roadMan.InputHandler();

            foreach (var drawable in drawables) {
                drawable.Draw();
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
}