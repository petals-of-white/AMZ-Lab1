using System.Drawing;
using Lab1.Models.Tools.ROI;
using OpenTK.Graphics.OpenGL;

namespace Lab1.Views.Tools.ROI;

public class RectangleROITool : ROITool
{
    public override bool IsDisplayed => Tool.IsDisplayed;
    public override PrimitiveType PrimitiveType => PrimitiveType.LineLoop;
    public RectangleROI Tool { get; set; } = new();
    public float LineThickness { get; } = 0.01f;

    public static (PointF left, PointF right) FromCenter(PointF center, float distance)
    {
        var left = center - new SizeF(distance / 2, 0);
        var right = center + new SizeF(distance / 2, 0);

        return (left, right);
    }
    public static PointF[] PrimitivesFromRectangle (Models.Shapes.Rectangle rectangle)
    {
        (var p1, var p2) = (rectangle.P1, rectangle.P2);

    }
    protected override PointF [] Contour =>
        Tool.Region is Models.Shapes.Rectangle
        {
            P1: PointF { X: var x1, Y: var y1 },
            P2: PointF { X: var x2, Y: var y2 }
        }
        ? [new(x1, y1), new(x1, y2), new(x2, y2), new(x2, y1)]
        : Array.Empty<PointF>();

    protected override PointF [] ReferencePoints => Contour;
}