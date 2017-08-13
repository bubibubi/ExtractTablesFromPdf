using System;

namespace BuildTablesFromPdf.Engine
{
    public interface IPageContent
    {
        void AddText(Point point, string content);
        bool Contains(Point point);
        bool Contains(double y);
        double Y { get; }
    }
}