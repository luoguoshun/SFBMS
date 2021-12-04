using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Common.EnumList
{
    /// <summary>
    /// 借阅状态
    /// </summary>
    public enum ExamineType
    {
        审核中 = 1,
        已通过 = 2,
        驳回 = 3,
        用户取消 = 4,
        已逾期 = 5,
        已归还 = 6,
        已支付 = 7
    }
}
