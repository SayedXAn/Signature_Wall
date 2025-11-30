using System;
using System.Collections.Concurrent;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    // A concurrent queue to store actions that need to be executed on the main thread
    private static readonly ConcurrentQueue<Action> _executionQueue = new ConcurrentQueue<Action>();

    // Enqueue an action to be executed on the main thread
    public static void Enqueue(Action action)
    {
        Debug.Log("Enqueuing action to be executed on the main thread");
        // Add the action to the queue
        _executionQueue.Enqueue(action);
    }

    // Execute actions in the queue during the Update() method
    private void Update()
    {
        // Process all actions in the queue (only executes on the main thread)
        while (_executionQueue.TryDequeue(out var action))
        {
            Debug.Log("Executing action on the main thread");
            // Execute the action
            action.Invoke();
        }
    }
}
