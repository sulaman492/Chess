public class Stack<T>
{
    private Node<T> head;

    public void Push(T data)
    {
        Node<T> newNode = new Node<T>(data);
        newNode.Next = head;
        head = newNode;
    }

    public T Pop()
    {
        if (head == null)
            throw new InvalidOperationException("Stack is empty"); // safer than returning null/default

        T data = head.Data;
        head = head.Next;
        return data;
    }

    public T Top()
    {
        if (head == null)
            throw new InvalidOperationException("Stack is empty");

        return head.Data;
    }


    public bool IsEmpty()
    {
        return head == null;
    }
    public void Clear()
    {
        head = null;    
    }

}
