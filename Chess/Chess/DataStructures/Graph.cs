using Chess.Moves;
using System.Collections.Generic;

namespace Chess.DataStructures
{
    public class Graph<T>
    {
        public GraphNode<T> Root { get; }
        public GraphNode<T> Current { get; private set; }

        public Graph(T rootData)
        {
            Root = new GraphNode<T> { Data = rootData };
            Current = Root;
        }

        // Add a move from current position
        public void AddMove(Move move, T newPositionData)
        {
            // Check if this move already exists (variation merge)
            foreach (var edge in Current.Edges)
            {
                if (edge.Move.Equals(move))
                {
                    Current = edge.To;
                    return;
                }
            }

            // Create new node
            GraphNode<T> newNode = new GraphNode<T>
            {
                Data = newPositionData,
                Parent = Current
            };

            Current.Edges.Add(new GraphEdge<T>
            {
                Move = move,
                To = newNode
            });

            Current = newNode;
        }

        // Undo in graph
        public void MoveBack()
        {
            if (Current.Parent != null)
                Current = Current.Parent;
        }

        // Get played moves for UI
        public List<Move> GetPlayedMoves()
        {
            List<Move> moves = new();
            var node = Current;

            while (node.Parent != null)
            {
                var parent = node.Parent;

                var edge = parent.Edges
                                 .Find(e => e.To == node);

                if (edge != null)
                    moves.Add(edge.Move);

                node = parent;
            }

            moves.Reverse();
            return moves;
        }
    }
}
