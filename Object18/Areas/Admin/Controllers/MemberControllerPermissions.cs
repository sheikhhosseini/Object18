using System.ComponentModel;

namespace Object18.Areas.Admin.Controllers;

[DisplayName("عضو")]
public class MemberControllerPermissions : IControllerPermissions
{
    private const string AreaName = "Admin.";
    private const string ControllerName = "Member.";

    [Description("نمایش فهرست")]
    public const string List = AreaName + ControllerName + nameof(List);

    [Description("ایجاد")]
    public const string Create = AreaName + ControllerName + nameof(Create);

    [Description("ویرایش")]
    public const string Update = AreaName + ControllerName + nameof(Update);

    [Description("حذف")]
    public const string Delete = AreaName + ControllerName + nameof(Delete);
}

public interface IControllerPermissions
{

}
