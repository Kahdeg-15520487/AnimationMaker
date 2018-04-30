using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Utilities
{
    namespace Drawing
    {
        public static class DrawingHelper
        {
            public abstract class Shape { public bool IsVisible { get; set; } = true; }

            public class Point : Shape
            {
                public Vector2 position;
                public Color color;
                public Point(Vector2 p, Color c) { position = p; color = c; }
            }

            public class Line : Shape
            {
                public Vector2 start;
                public Vector2 end;
                public Color color;
                public float thickness;
                public Line(Vector2 s, Vector2 e, Color c, float t) { start = s; end = e; color = c; thickness = t; }
            }

            public class Rectangle : Shape
            {
                public Microsoft.Xna.Framework.Rectangle rectangle;
                public Color color;
                public float thickness;
                public bool fill;
                public Rectangle(Microsoft.Xna.Framework.Rectangle r, Color c, float t, bool f) { rectangle = r; color = c; thickness = t; fill = f; }
            }

            public class Circle : Shape
            {
                public Vector2 center;
                public float radius;
                public int side;
                public Color color;
                public float thickness;
                public Circle(Vector2 ce, float r, int s, Color c, float t) { center = ce; radius = r; side = s; color = c; thickness = t; }
            }

            static List<Shape> shape_list = new List<Shape>();

            public static void ClearShapeList()
            {
                shape_list.Clear();
            }

            /// <summary>
            /// Add a shape for drawing
            /// </summary>
            /// <param name="shape"></param>
            public static void DrawShape(Shape shape)
            {
                shape_list.Add(shape);
            }

            /// <summary>
            /// check if shape is already added
            /// </summary>
            /// <param name="shape"></param>
            /// <returns></returns>
            public static bool ContainShape(Shape shape)
            {
                return shape_list.Contains(shape);
            }

            public static void RemoveShape(Shape shape)
            {
                shape_list.Remove(shape);
            }

            /// <summary>
            /// Add a point for drawing
            /// </summary>
            /// <param name="point"></param>
            /// <param name="color"></param>
            public static void DrawPoint(Vector2 point, Color color)
            {
                shape_list.Add(new Point(point, color));
            }

            /// <summary>
            /// Instantiate a point
            /// </summary>
            /// <param name="point"></param>
            /// <param name="color"></param>
            /// <returns></returns>
            public static Point GetPoint(Vector2 point, Color color)
            {
                return new Point(point, color);
            }

            public static void DrawLine(Vector2 start, Vector2 end, Color color, float thickness)
            {
                shape_list.Add(new Line(start, end, color, thickness));
            }

            public static Line GetLine(Vector2 start, Vector2 end, Color color, float thickness)
            {
                return new Line(start, end, color, thickness);
            }

            public static void DrawRectangle(Microsoft.Xna.Framework.Rectangle rect, Color color, bool fill, float thickness = 1f)
            {
                shape_list.Add(new Rectangle(rect, color, thickness, fill));
            }

            public static Rectangle GetRectangle(Microsoft.Xna.Framework.Rectangle rect, Color color, bool fill, float thickness = 1f)
            {
                return new Rectangle(rect, color, thickness, fill);
            }

            public static void DrawRectangle(Microsoft.Xna.Framework.Point position, Microsoft.Xna.Framework.Point size, Color color, bool fill, float thickness = 1f)
            {
                shape_list.Add(new Rectangle(new Microsoft.Xna.Framework.Rectangle(position, size), color, thickness, fill));
            }

            public static Rectangle GetRectangle(Microsoft.Xna.Framework.Point position, Microsoft.Xna.Framework.Point size, Color color, bool fill, float thickness = 1f)
            {
                return new Rectangle(new Microsoft.Xna.Framework.Rectangle(position, size), color, thickness, fill);
            }

            public static void DrawRectangle(int x1, int y1, int x2, int y2, Color color, bool fill, float thickness = 1f)
            {
                shape_list.Add(new Rectangle(new Microsoft.Xna.Framework.Rectangle(x1, y1, x2, y2), color, thickness, fill));
            }

            public static Rectangle GetRectangle(int x1, int y1, int x2, int y2, Color color, bool fill, float thickness = 1f)
            {
                return new Rectangle(new Microsoft.Xna.Framework.Rectangle(x1, y1, x2, y2), color, thickness, fill);
            }

            public static void DrawCircle(Vector2 center, float radius, Color color, float thickness = 1f)
            {
                shape_list.Add(new Circle(center, radius, 1, color, thickness));
            }

            public static Circle GetCircle(Vector2 center, float radius, Color color, float thickness = 1f)
            {
                return new Circle(center, radius, 1, color, thickness);
            }

            public static void Draw(SpriteBatch spriteBatch)
            {
                foreach (Shape shape in shape_list)
                {
                    if (shape != null && shape.IsVisible)
                    {
                        if (shape is Point p)
                        {
                            spriteBatch.PutPixel(p.position, p.color);
                        }
                        else if (shape is Line l)
                        {
                            spriteBatch.DrawLine(l.start, l.end, l.color, l.thickness);
                        }
                        else if (shape is Rectangle r)
                        {
                            spriteBatch.DrawRectangle(r.rectangle, r.color, r.thickness);
                        }
                        else if (shape is Circle c)
                        {
                            spriteBatch.DrawCircle(c.center, c.radius, c.side, c.color, c.thickness);
                        }
                    }
                }
            }
        }
    }
}