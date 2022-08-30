using System.ComponentModel;

namespace Object18.Controllers;

[DisplayName("سفارش خرید")]
public class TestControllerPermissions
{
    private const string AreaName = "Admin.";
    private const string ControllerName = "Home.";

    [Description("پرایویسی")]
    public const string Privacy = AreaName + ControllerName + nameof(Privacy);

    //[Description("نمایش فهرست")]
    //public const string List = AreaName + ControllerName + nameof(List);

    //[Description("ایجاد")]
    //public const string Create = AreaName + ControllerName + nameof(Create);

    //[Description("ویرایش")]
    //public const string Update = AreaName + ControllerName + nameof(Update);

    //[Description("حذف")]
    //public const string Delete = AreaName + ControllerName + nameof(Delete);
}
