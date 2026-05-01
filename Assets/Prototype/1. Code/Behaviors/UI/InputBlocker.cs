using UnityEngine;

public static class InputBlocker
{
    public static bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}
