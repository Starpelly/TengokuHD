﻿using System.Runtime.InteropServices;
using Raylib_CsLo;

using ImGuiNET;

namespace Trinkit
{
    public class TrinkitImGui
    {
        internal static IntPtr ImGuiContext = IntPtr.Zero;

        private static ImGuiMouseCursor? CurrentMouseCursor = ImGuiMouseCursor.COUNT;
        private static Dictionary<ImGuiMouseCursor, MouseCursor>? MouseCursorMap;
        private static KeyboardKey[]? KeyEnumMap;

        private static Texture FontTexture;

        public static void Setup(bool darkTheme = true)
        {
            MouseCursorMap = new Dictionary<ImGuiMouseCursor, MouseCursor>();
            KeyEnumMap = Enum.GetValues(typeof(KeyboardKey)) as KeyboardKey[];

            FontTexture.id = 0;

            BeginInitImGui();

            if (darkTheme)
                ImGui.StyleColorsDark();
            else
                ImGui.StyleColorsLight();

            EndInitImGui();
        }

        public static void BeginInitImGui()
        {
            ImGuiContext = ImGui.CreateContext();
        }

        private static void SetupMouseCursors()
        {
            if (MouseCursorMap == null) return;
            MouseCursorMap.Clear();
            MouseCursorMap[ImGuiMouseCursor.Arrow] = MouseCursor.MOUSE_CURSOR_ARROW;
            MouseCursorMap[ImGuiMouseCursor.TextInput] = MouseCursor.MOUSE_CURSOR_IBEAM;
            MouseCursorMap[ImGuiMouseCursor.Hand] = MouseCursor.MOUSE_CURSOR_POINTING_HAND;
            MouseCursorMap[ImGuiMouseCursor.ResizeAll] = MouseCursor.MOUSE_CURSOR_RESIZE_ALL;
            MouseCursorMap[ImGuiMouseCursor.ResizeEW] = MouseCursor.MOUSE_CURSOR_RESIZE_EW;
            MouseCursorMap[ImGuiMouseCursor.ResizeNESW] = MouseCursor.MOUSE_CURSOR_RESIZE_NESW;
            MouseCursorMap[ImGuiMouseCursor.ResizeNS] = MouseCursor.MOUSE_CURSOR_RESIZE_NS;
            MouseCursorMap[ImGuiMouseCursor.ResizeNWSE] = MouseCursor.MOUSE_CURSOR_RESIZE_NWSE;
            MouseCursorMap[ImGuiMouseCursor.NotAllowed] = MouseCursor.MOUSE_CURSOR_NOT_ALLOWED;
        }

        public static unsafe void ReloadFonts()
        {
            ImGui.SetCurrentContext(ImGuiContext);
            // ImGuizmo.SetImGuiContext(ImGuiContext);

            ImGuiIOPtr io = ImGui.GetIO();

            int width, height, bytesPerPixel;
            io.Fonts.GetTexDataAsRGBA32(out byte* pixels, out width, out height, out bytesPerPixel);

            Image image = new Image
            {
                data = pixels,
                width = width,
                height = height,
                mipmaps = 1,
                format = (int)PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8,
            };

            FontTexture = Raylib.LoadTextureFromImage(image);

            io.Fonts.SetTexID(new IntPtr(FontTexture.id));
        }

        public static void EndInitImGui()
        {
            SetupMouseCursors();

            ImGui.SetCurrentContext(ImGuiContext);
            // ImGuizmo.SetImGuiContext(ImGuiContext);

            var fonts = ImGui.GetIO().Fonts;
            ImGui.GetIO().Fonts.AddFontDefault();

            ImGuiIOPtr io = ImGui.GetIO();
            io.KeyMap[(int)ImGuiKey.Tab] = (int)KeyboardKey.KEY_TAB;
            io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)KeyboardKey.KEY_LEFT;
            io.KeyMap[(int)ImGuiKey.RightArrow] = (int)KeyboardKey.KEY_RIGHT;
            io.KeyMap[(int)ImGuiKey.UpArrow] = (int)KeyboardKey.KEY_UP;
            io.KeyMap[(int)ImGuiKey.DownArrow] = (int)KeyboardKey.KEY_DOWN;
            io.KeyMap[(int)ImGuiKey.PageUp] = (int)KeyboardKey.KEY_PAGE_UP;
            io.KeyMap[(int)ImGuiKey.PageDown] = (int)KeyboardKey.KEY_PAGE_DOWN;
            io.KeyMap[(int)ImGuiKey.Home] = (int)KeyboardKey.KEY_HOME;
            io.KeyMap[(int)ImGuiKey.End] = (int)KeyboardKey.KEY_END;
            io.KeyMap[(int)ImGuiKey.Delete] = (int)KeyboardKey.KEY_DELETE;
            io.KeyMap[(int)ImGuiKey.Backspace] = (int)KeyboardKey.KEY_BACKSPACE;
            io.KeyMap[(int)ImGuiKey.Enter] = (int)KeyboardKey.KEY_ENTER;
            io.KeyMap[(int)ImGuiKey.Escape] = (int)KeyboardKey.KEY_ESCAPE;
            io.KeyMap[(int)ImGuiKey.Space] = (int)KeyboardKey.KEY_SPACE;
            io.KeyMap[(int)ImGuiKey.A] = (int)KeyboardKey.KEY_A;
            io.KeyMap[(int)ImGuiKey.C] = (int)KeyboardKey.KEY_C;
            io.KeyMap[(int)ImGuiKey.V] = (int)KeyboardKey.KEY_V;
            io.KeyMap[(int)ImGuiKey.X] = (int)KeyboardKey.KEY_X;
            io.KeyMap[(int)ImGuiKey.Y] = (int)KeyboardKey.KEY_Y;
            io.KeyMap[(int)ImGuiKey.Z] = (int)KeyboardKey.KEY_Z;

            ReloadFonts();
        }

        private static void NewFrame()
        {
            ImGuiIOPtr io = ImGui.GetIO();

            if (Raylib.IsWindowFullscreen())
            {
                int monitor = Raylib.GetCurrentMonitor();
                io.DisplaySize = new System.Numerics.Vector2(Raylib.GetMonitorWidth(monitor), Raylib.GetMonitorHeight(monitor));
            }
            else
            {
                io.DisplaySize = new System.Numerics.Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            }

            io.DisplayFramebufferScale = new System.Numerics.Vector2(1, 1);
            io.DeltaTime = Raylib.GetFrameTime();

            io.KeyCtrl = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_CONTROL) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL);
            io.KeyShift = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SHIFT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT);
            io.KeyAlt = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_ALT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_ALT);
            io.KeySuper = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SUPER) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SUPER);

            if (io.WantSetMousePos)
            {
                Raylib.SetMousePosition((int)io.MousePos.X, (int)io.MousePos.Y);
            }
            else
            {
                io.MousePos = Raylib.GetMousePosition();
            }

            io.MouseDown[0] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT);
            io.MouseDown[1] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT);
            io.MouseDown[2] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_MIDDLE);

            if (Raylib.GetMouseWheelMove() > 0)
                io.MouseWheel += 1;
            else if (Raylib.GetMouseWheelMove() < 0)
                io.MouseWheel -= 1;

            if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) == 0)
            {
                ImGuiMouseCursor imgui_cursor = ImGui.GetMouseCursor();
                if (imgui_cursor != CurrentMouseCursor || io.MouseDrawCursor)
                {
                    CurrentMouseCursor = imgui_cursor;
                    if (io.MouseDrawCursor || imgui_cursor == ImGuiMouseCursor.None)
                    {
                        Raylib.HideCursor();
                    }
                    else
                    {
                        Raylib.ShowCursor();

                        if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) == 0)
                        {
                            if (MouseCursorMap != null)
                            {
                                if (!MouseCursorMap.ContainsKey(imgui_cursor))
                                    Raylib.SetMouseCursor(MouseCursor.MOUSE_CURSOR_DEFAULT);
                                else
                                    Raylib.SetMouseCursor(MouseCursorMap[imgui_cursor]);
                            }
                        }
                    }
                }
            }
        }


        private static void FrameEvents()
        {
            ImGuiIOPtr io = ImGui.GetIO();

            if (KeyEnumMap != null)
                foreach (KeyboardKey key in KeyEnumMap)
                {
                    io.KeysDown[(int)key] = Raylib.IsKeyDown(key);
                }

            uint pressed = (uint)Raylib.GetCharPressed();
            while (pressed != 0)
            {
                io.AddInputCharacter(pressed);
                pressed = (uint)Raylib.GetCharPressed();
            }
        }

        public static void Begin()
        {
            ImGui.SetCurrentContext(ImGuiContext);
            // ImGuizmo.SetImGuiContext(ImGuiContext);


            NewFrame();
            FrameEvents();
            ImGui.NewFrame();
            // ImGuizmo.BeginFrame();
        }

        private static void EnableScissor(float x, float y, float width, float height)
        {
            RlGl.rlEnableScissorTest();
            RlGl.rlScissor((int)x, Raylib.GetScreenHeight() - (int)(y + height), (int)width, (int)height);
        }

        private static void TriangleVert(ImDrawVertPtr idx_vert)
        {
            System.Numerics.Vector4 color = ImGui.ColorConvertU32ToFloat4(idx_vert.col);

            RlGl.rlColor4f(color.X, color.Y, color.Z, color.W);
            RlGl.rlTexCoord2f(idx_vert.uv.X, idx_vert.uv.Y);
            RlGl.rlVertex2f(idx_vert.pos.X, idx_vert.pos.Y);
        }

        private static void RenderTriangles(uint count, uint indexStart, ImVector<ushort> indexBuffer, ImPtrVector<ImDrawVertPtr> vertBuffer, IntPtr texturePtr)
        {
            if (count < 3)
                return;

            uint textureId = 0;
            if (texturePtr != IntPtr.Zero)
                textureId = (uint)texturePtr.ToInt32();

            RlGl.rlBegin(RlGl.RL_TRIANGLES);
            RlGl.rlSetTexture(textureId);

            for (int i = 0; i <= (count - 3); i += 3)
            {
                if (RlGl.rlCheckRenderBatchLimit(3))
                {
                    RlGl.rlBegin(RlGl.RL_TRIANGLES);
                    RlGl.rlSetTexture(textureId);
                }

                ushort indexA = indexBuffer[(int)indexStart + i];
                ushort indexB = indexBuffer[(int)indexStart + i + 1];
                ushort indexC = indexBuffer[(int)indexStart + i + 2];

                ImDrawVertPtr vertexA = vertBuffer[indexA];
                ImDrawVertPtr vertexB = vertBuffer[indexB];
                ImDrawVertPtr vertexC = vertBuffer[indexC];

                TriangleVert(vertexA);
                TriangleVert(vertexB);
                TriangleVert(vertexC);
            }
            RlGl.rlEnd();
        }

        private delegate void Callback(ImDrawListPtr list, ImDrawCmdPtr cmd);

        private static void RenderData()
        {
            RlGl.rlDrawRenderBatchActive();
            RlGl.rlDisableBackfaceCulling();

            var data = ImGui.GetDrawData();

            for (int l = 0; l < data.CmdListsCount; l++)
            {
                ImDrawListPtr commandList = data.CmdListsRange[l];

                for (int cmdIndex = 0; cmdIndex < commandList.CmdBuffer.Size; cmdIndex++)
                {
                    var cmd = commandList.CmdBuffer[cmdIndex];

                    EnableScissor(cmd.ClipRect.X - data.DisplayPos.X, cmd.ClipRect.Y - data.DisplayPos.Y, cmd.ClipRect.Z - (cmd.ClipRect.X - data.DisplayPos.X), cmd.ClipRect.W - (cmd.ClipRect.Y - data.DisplayPos.Y));
                    if (cmd.UserCallback != IntPtr.Zero)
                    {
                        Callback cb = Marshal.GetDelegateForFunctionPointer<Callback>(cmd.UserCallback);
                        cb(commandList, cmd);
                        continue;
                    }

                    RenderTriangles(cmd.ElemCount, cmd.IdxOffset, commandList.IdxBuffer, commandList.VtxBuffer, cmd.TextureId);

                    RlGl.rlDrawRenderBatchActive();
                }
            }
            RlGl.rlSetTexture(0);
            RlGl.rlDisableScissorTest();
            RlGl.rlEnableBackfaceCulling();
        }

        public static void End()
        {
            ImGui.SetCurrentContext(ImGuiContext);
            // ImGuizmo.SetImGuiContext(ImGuiContext);

            ImGui.Render();
            RenderData();
        }

        public static void Shutdown()
        {
            Raylib.UnloadTexture(FontTexture);
            ImGui.DestroyContext();
        }

        public static void Image(Texture image)
        {
            ImGui.Image(new IntPtr(image.id), new System.Numerics.Vector2(image.width, image.height));
        }

        public static void ImageSize(Texture image, int width, int height)
        {
            ImGui.Image(new IntPtr(image.id), new System.Numerics.Vector2(width, height));
        }

        public static void ImageSize(Texture image, System.Numerics.Vector2 size)
        {
            ImGui.Image(new IntPtr(image.id), size);
        }

        public static void ImageSize(Texture image, System.Numerics.Vector2 size, Color color)
        {
            ImGui.Image(new IntPtr(image.id), size, new System.Numerics.Vector2(0, 0), new System.Numerics.Vector2(1, 1), new System.Numerics.Vector4(color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f));
        }

        public static void ImageRect(Texture image, int destWidth, int destHeight, Rectangle sourceRect)
        {
            System.Numerics.Vector2 uv0 = new System.Numerics.Vector2();
            System.Numerics.Vector2 uv1 = new System.Numerics.Vector2();

            if (sourceRect.width < 0)
            {
                uv0.X = -((float)sourceRect.x / image.width);
                uv1.X = (uv0.X - (float)(Math.Abs(sourceRect.width) / image.width));
            }
            else
            {
                uv0.X = (float)sourceRect.x / image.width;
                uv1.X = uv0.X + (float)(sourceRect.width / image.width);
            }

            if (sourceRect.height < 0)
            {
                uv0.Y = -((float)sourceRect.y / image.height);
                uv1.Y = (uv0.Y - (float)(Math.Abs(sourceRect.height) / image.height));
            }
            else
            {
                uv0.Y = (float)sourceRect.y / image.height;
                uv1.Y = uv0.Y + (float)(sourceRect.height / image.height);
            }

            ImGui.Image(new IntPtr(image.id), new System.Numerics.Vector2(destWidth, destHeight), uv0, uv1);
        }
    }
}