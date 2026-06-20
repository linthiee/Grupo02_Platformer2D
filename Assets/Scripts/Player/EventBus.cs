using System;
using System.Collections.Generic;

public struct EasyLevelEvent {}
public struct MediumLevelEvent {}
public struct HardLevelEvent {}

public struct ExitToMenuEvent {}

public struct PerfectLandEvent
{
    public int points;
}
public struct BlockLandedEvent 
{
    public bool groundLand;
    public bool blockLand;

    public int points;
}

public struct UpdateCanvasEvent { }
public struct EndGameEvent { }
public struct TowerFallEvent { }

public interface IEventBus
{
    void Subscribe<T>(Action<T> onEvent);
    void Unsubscribe<T>(Action<T> onEvent);
    void Publish<T>(T eventData);
}

public class EventBus : IEventBus
{
    private readonly Dictionary<Type, Delegate> _subscribers = new Dictionary<Type, Delegate>();
    public void Subscribe<T>(Action<T> onEvent)
    {
        var type = typeof(T);
        if (_subscribers.ContainsKey(type))
            _subscribers[type] = Delegate.Combine(_subscribers[type], onEvent);
        else
            _subscribers[type] = onEvent;
    }

    public void Unsubscribe<T>(Action<T> onEvent)
    {
        var type = typeof(T);
        if (_subscribers.ContainsKey(type))
            _subscribers[type] = Delegate.Remove(_subscribers[type], onEvent);
    }

    public void Publish<T>(T eventData)
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var action))
            (action as Action<T>)?.Invoke(eventData);
    }
}