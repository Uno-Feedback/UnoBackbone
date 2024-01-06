using System.Globalization;
using Uno.Domain.Entities;

namespace Uno.Domain.Extentions;

public static class IssueExtentions
{
    public static string GenerateDescription(this Issue issue)
    {
        PersianCalendar persianCalendar = new();

        return $@"
            <table border=""1"" class=""jeditorTable"" style=""width:100%"">
                <tbody>
                    <tr>
                        <td>
                            <h3><span style=""color:#2c3e50""><b>ثبت کننده</b></span></h3>
                        </td>
                        <td>
                            <h3><span style=""color:#2c3e50""><b>تاریخ ارسال</b></span></h3>
                        </td>
                        <td>
                            <h3><span style=""color:#2c3e50""><b>آدرس</b></span></h3>
                        </td>
                    </tr>
                    <tr>
                        <td>{issue.Reporter}</td>
                        <td>{persianCalendar.GetYear(DateTime.Now)}:{persianCalendar.GetMonth(DateTime.Now)}:{persianCalendar.GetDayOfMonth(DateTime.Now)}</td>
                        <td>{issue.Url}</td>
                    </tr>
                </tbody>
            </table>

            <p>&nbsp;</p>

            <p> توضیحات ارسالی </p>

            <p>{issue.Description}</p>";
    }
}