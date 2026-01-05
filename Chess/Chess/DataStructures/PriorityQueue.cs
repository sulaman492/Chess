using System;
using System.Collections.Generic;

namespace Chess.DataStructures
{
    public class PriorityQueue<T>
    {
        private List<(T Item, int Priority)> heap = new();

        public int Count => heap.Count;

        public void Enqueue(T item, int priority)
        {
            heap.Add((item, priority));
            HeapifyUp(heap.Count - 1);
        }

        public T Dequeue()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Queue is empty");

            T root = heap[0].Item;
            heap[0] = heap[^1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);

            return root;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (heap[index].Priority <= heap[parent].Priority)
                    break;

                (heap[index], heap[parent]) = (heap[parent], heap[index]);
                index = parent;
            }
        }

        private void HeapifyDown(int index)
        {
            int lastIndex = heap.Count - 1;

            while (true)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int largest = index;

                if (left <= lastIndex && heap[left].Priority > heap[largest].Priority)
                    largest = left;

                if (right <= lastIndex && heap[right].Priority > heap[largest].Priority)
                    largest = right;

                if (largest == index)
                    break;

                (heap[index], heap[largest]) = (heap[largest], heap[index]);
                index = largest;
            }
        }
    }
}
