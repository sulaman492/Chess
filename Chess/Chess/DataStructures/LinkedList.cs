namespace Chess.DataStructures
{
    public class LinkedList<T>
    {
        public Node<T> Head { get; private set; }
        public Node<T> Tail { get; private set; }
        public int Count { get; private set; }

        public LinkedList()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }

        // Add at end (for replay in order)
        public void AddLast(T data)
        {
            Node<T> newNode = new Node<T>(data);

            if (Head == null)
            {
                Head = newNode;
                Tail = newNode;
            }
            else
            {
                Tail.Next = newNode;
                newNode.Previous = Tail; // set previous
                Tail = newNode;
            }

            Count++;
        }


        // Optional: add at beginning
        public void AddFirst(T data)
        {
            Node<T> newNode = new Node<T>(data);
            newNode.Next = Head;
            Head = newNode;

            if (Tail == null)
                Tail = Head;

            Count++;
        }

        // Traverse (core for Review)
        public IEnumerable<T> Traverse()
        {
            Node<T> current = Head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        // Clear list (for Restart)
        public void Clear()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }
    }
}
