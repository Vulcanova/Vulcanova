namespace Vulcanova.Core.Uonet;

public interface IWidgetUpdater<in TEvent> where TEvent : UonetDataUpdatedEvent
{
    void Handle(TEvent message);
}