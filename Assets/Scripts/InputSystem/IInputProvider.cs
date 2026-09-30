using System;
using UnityEngine;

namespace BeastLinkBattle.InputSystem
{
    public interface IInputProvider
    {
        event Action<Vector3> OnPointerDown;
        void EnableInput(bool isEnabled);
    }
}