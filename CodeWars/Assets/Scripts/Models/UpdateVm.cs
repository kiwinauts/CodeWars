using UnityEngine;

[System.Serializable]
public class UpdateVm
{
    public UpdateType updateType;

    public string Name;

    public float MinimumUpdate;

    public float MaximumUpdate;

    public Attack Attack;

    private float? updateValue;

    public Sprite Image;

    public float UpdateValue
    {
        get
        {
            if (updateValue == null)
            {
                updateValue = Random.Range(MinimumUpdate, MaximumUpdate);
            }

            return updateValue.Value;
        }
    }
}
