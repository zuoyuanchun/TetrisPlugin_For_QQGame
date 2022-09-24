using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace TetrisMonitor
{
    public static class RichTextBoxExtension
    {
        /// <summary>
        /// 富文本编辑器扩展方法
        /// </summary>
        /// <param name="rtBox">默认</param>
        /// <param name="text">添加的文本内容</param>
        /// <param name="color">文本颜色</param>
        /// <param name="addNewLine">是否换行</param>
        public static void ATC(this RichTextBox rtBox, string text, Color color, bool addNewLine = true)
        {
            if (addNewLine)
            {
                text += Environment.NewLine;
            }
            rtBox.SelectionStart = rtBox.TextLength;
            rtBox.SelectionLength = 0;
            rtBox.SelectionColor = color;
            rtBox.AppendText(text);
            rtBox.SelectionColor = rtBox.ForeColor;
        }
    }
}
