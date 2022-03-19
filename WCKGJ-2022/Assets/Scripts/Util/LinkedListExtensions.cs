using System;
using System.Collections.Generic;

internal static class LinkedListExtensions
{
    public static void ForEach<T>(this LinkedList<T> source, Action<T> action)
    {
        var nextNode = source.First;
        while (nextNode is not null)
        {
            var node = nextNode;
            nextNode = nextNode.Next;

            action(node.Value);
        }
    }
}
