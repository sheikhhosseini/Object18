using System.ComponentModel;

namespace Object18.Controllers;

[DisplayName("سفارش خرید")]
public class TestControllerPermissions
{
    private const string ApplicationName = "AssetInventory.";
    private const string ControllerName = "PurchaseOrder.";

    [Description("نمایش فهرست")]
    public const string List = ApplicationName + ControllerName + nameof(List);

    [Description("ایجاد")]
    public const string Create = ApplicationName + ControllerName + nameof(Create);

    [Description("ویرایش")]
    public const string Update = ApplicationName + ControllerName + nameof(Update);

    [Description("حذف")]
    public const string Delete = ApplicationName + ControllerName + nameof(Delete);
}
