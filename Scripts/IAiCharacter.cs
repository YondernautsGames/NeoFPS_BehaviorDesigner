namespace NeoFPS.BehaviourDesigner
{
    public interface IAiCharacter
    {
        IQuickSlots quickSlots { get; }
        IInventory inventory { get; }
    }
}