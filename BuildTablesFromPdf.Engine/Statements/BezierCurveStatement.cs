using System.Globalization;

namespace BuildTablesFromPdf.Engine.Statements
{
    /// <summary>
    /// Draw a bezier curve from the current point to the specified point using intermediate points
    /// </summary>
    /// <seealso cref="PointStatement" />
    class BezierCurveStatement : SingleLineStatement
    {
        public BezierCurveStatement(string rawContent)
        {
            RawContent = rawContent;
            float x = float.Parse(rawContent.Split(' ')[4], NumberFormatInfo.InvariantInfo);
            float y = float.Parse(rawContent.Split(' ')[5], NumberFormatInfo.InvariantInfo);

            ToPoint = new Point(x, y);
        }

        public readonly Point ToPoint;

    }
}