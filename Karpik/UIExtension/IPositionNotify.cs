using UnityEngine;
using UnityEngine.UIElements;

namespace Karpik.UIExtension
{
    public interface IPositionNotify : INotifyValueChanged<Vector2>
    {
        public Vector2 Center { get; set; }
    }
}
