using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
    private readonly List<T> items = new List<T>();

    public int Count => items.Count;

    public void Enqueue(T item)
    {
        items.Add(item);
        int childIndex = items.Count - 1;

        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;

            if (items[childIndex].CompareTo(items[parentIndex]) >= 0)
                break;

            Swap(childIndex, parentIndex);
            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        if (items.Count == 0)
            throw new InvalidOperationException("Priority queue is empty.");

        T frontItem = items[0];
        int lastIndex = items.Count - 1;

        items[0] = items[lastIndex];
        items.RemoveAt(lastIndex);

        if (items.Count == 0)
            return frontItem;

        int parentIndex = 0;

        while (true)
        {
            int leftChildIndex = parentIndex * 2 + 1;
            if (leftChildIndex >= items.Count)
                break;

            int rightChildIndex = leftChildIndex + 1;
            int smallestChildIndex = leftChildIndex;

            if (rightChildIndex < items.Count &&
                items[rightChildIndex].CompareTo(items[leftChildIndex]) < 0)
            {
                smallestChildIndex = rightChildIndex;
            }

            if (items[parentIndex].CompareTo(items[smallestChildIndex]) <= 0)
                break;

            Swap(parentIndex, smallestChildIndex);
            parentIndex = smallestChildIndex;
        }

        return frontItem;
    }

    private void Swap(int firstIndex, int secondIndex)
    {
        T temp = items[firstIndex];
        items[firstIndex] = items[secondIndex];
        items[secondIndex] = temp;
    }
}
