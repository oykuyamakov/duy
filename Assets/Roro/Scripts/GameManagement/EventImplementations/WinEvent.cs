using Events;
using UnityEngine;

namespace Roro.Scripts.GameManagement.EventImplementations
{
    public class WinEvent : Event<WinEvent>
    {
        public static WinEvent Get(bool result)
        {
            var evt = GetPooledInternal();
            return evt;
        }
    }
}