using System.Reactive;
using ReactiveUI;

namespace Vulcanova.Core.Rx
{
    public class Interactions
    {
        public static readonly Interaction<ExceptionDescriptor, Unit> Errors = new();
    }
}