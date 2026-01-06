using Chess.Moves;

namespace Chess.DataStructures
{
    public class GraphNode<T>
    {
        public T Data;
        public List<GraphEdge<T>> Edges = new();
        public GraphNode<T>? Parent;
    }

    public class GraphEdge<T>
    {
        public Move Move;
        public GraphNode<T> To;
    }
}
