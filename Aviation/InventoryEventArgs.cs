namespace com.ntier.Aviation;

delegate void InventoryEventHandler(object sender, InventoryEventArgs e);

internal class InventoryEventArgs(string partNumber) : EventArgs
{
    public string PartNumber => partNumber;
}
