namespace BuildTablesFromPdf.Engine
{
    public class GraphicState
    {
        public Matrix TransformMatrix { get; set; }
        public Color Color { get; set; }
        
        public GraphicState Clone()
        {
            return new GraphicState()
            {
                TransformMatrix = TransformMatrix,
                Color = Color
            };
        }
    }
}