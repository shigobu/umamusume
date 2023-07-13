using System;

namespace UmamusumeAutoSize
{
    /// <summary>
    /// 設定保存用
    /// </summary>
    [Serializable]
    public class Setting
    {
        /// <summary>
        /// 前の横RECT
        /// </summary>
        public RECT BeforeHorizontalRECT { get; set; } = new RECT();
        /// <summary>
        /// 前の縦RECT
        /// </summary>
        public RECT BeforeVerticalRECT { get; set; } = new RECT();
    }
}
