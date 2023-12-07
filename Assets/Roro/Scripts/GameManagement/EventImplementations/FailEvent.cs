using Events;
using UnityEngine;

namespace Roro.Scripts.GameManagement.EventImplementations
{
    public class FailEvent : Event<FailEvent>
    {
        public static FailEvent Get(bool result)
        {
            var evt = GetPooledInternal();
            return evt;
        }
    }
}