namespace Abb.Euopc.SharedDesks.WebClient.Data;

public sealed class TreeItemData
{
    public TreeItemData? Parent { get; set; }

    public string Text { get; set; }

    public int? Value { get; set; }

    public bool IsExpanded { get; set; } = false;

    public bool IsChecked { get; set; } = false;

    public bool HasChildren => Children.Count > 0;

    public HashSet<TreeItemData> Children { get; set; } = new HashSet<TreeItemData>();

    public TreeItemData(string text, int? value = default!)
    {
        Text = text;
        Value = value;
    }

    public void AddChild(string itemName, int? value = default)
    {
        var item = new TreeItemData(itemName, value);
        item.Parent = this;
        Children.Add(item);
    }

    public void AddChild(TreeItemData item)
    {
        item.Parent = this;
        Children.Add(item);
    }

    public bool HasPartialChildSelection()
    {
        var childrenCheckedCount = Children.Where(c => c.IsChecked).Count();
        return HasChildren && childrenCheckedCount > 0 && childrenCheckedCount < Children.Count;
    }
}
