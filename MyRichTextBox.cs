using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WindowsFormsApplication9

{
    public class MyRichTextBox : RichTextBox
    {
        Panel panel1;

        public MyRichTextBox()
        {

            InitializeLineId();
        }
         
        

        private void InitializeLineId()
        {
            
            this.panel1 = new Panel();
            this.panel1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Width = 35;
            this.panel1.Dock = DockStyle.Left;
            this.SelectionIndent = 35;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            SetLineSpace(this, 350);
            this.LanguageOption = RichTextBoxLanguageOptions.AutoFont;
            this.Controls.Add(this.panel1);
           
        }
        //绘图显示行号
        public void showLineNumber()
        {
            this.Cursor = null;
            
            Point p = this.Location;
            int crntFirstIndex = this.GetCharIndexFromPosition(p);
            int crntFirstLine = this.GetLineFromCharIndex(crntFirstIndex);
            Point crntFirstPos = this.GetPositionFromCharIndex(crntFirstIndex);
            p.Y += this.Height;

            int crntLastIndex = this.GetCharIndexFromPosition(p);
            int crntLastLine = this.GetLineFromCharIndex(crntLastIndex);
            Point crntLastPos = this.GetPositionFromCharIndex(crntLastIndex);

            Graphics g = this.panel1.CreateGraphics();
            Bitmap bufferimage = new Bitmap(this.panel1.Width, this.panel1.Height);
            Graphics g1 = Graphics.FromImage(bufferimage);
            g1.Clear(this.BackColor);
            g1.SmoothingMode = SmoothingMode.HighQuality;
            g1.PixelOffsetMode = PixelOffsetMode.HighQuality; 

            Font font = new Font(this.Font, this.Font.Style);
            SolidBrush brush = new SolidBrush(Color.Green);

        
            Rectangle rect = this.panel1.ClientRectangle;
            brush.Color = this.panel1.BackColor;
            g1.FillRectangle(brush, 0, 0, this.panel1.ClientRectangle.Width, this.panel1.ClientRectangle.Height);
            brush.Color = Color.Green;
            int lineSpace = 0;

            if (crntFirstLine != crntLastLine)
            {
                lineSpace = (crntLastPos.Y - crntFirstPos.Y) / (crntLastLine - crntFirstLine);

            }

            else
            {
                lineSpace = Convert.ToInt32(this.Font.Size);

            }

            int brushX = this.panel1.ClientRectangle.Width - Convert.ToInt32(font.Size * 2.6);
            int brushY = crntLastPos.Y + Convert.ToInt32(font.Size * 0.1f);

            for (int i = crntLastLine; i >= crntFirstLine; i--)
            {

                g1.DrawString((i + 1).ToString(), font, brush, brushX, brushY);
                brushY -= lineSpace;
            }

            g.DrawImage(bufferimage, 0, 0);

            g1.Dispose();
            g.Dispose();
            font.Dispose();
            brush.Dispose();
          
        }
      
        protected override void OnTextChanged(EventArgs e)
        {
            showLineNumber();
            base.OnTextChanged(e);
            
        }
        protected override void OnVScroll(EventArgs e)
        {
            showLineNumber();
            base.OnVScroll(e);
          
        }


        public const int WM_USER = 0x0400;
        public const int EM_GETPARAFORMAT = WM_USER + 61;
        public const int EM_SETPARAFORMAT = WM_USER + 71;
        public const long MAX_TAB_STOPS = 32;
        public const uint PFM_LINESPACING = 0x00000100;
        [StructLayout(LayoutKind.Sequential)]
        private struct PARAFORMAT2
        {
            public int cbSize;
            public uint dwMask;
            public short wNumbering;
            public short wReserved;
            public int dxStartIndent;
            public int dxRightIndent;
            public int dxOffset;
            public short wAlignment;
            public short cTabCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] rgxTabs;
            public int dySpaceBefore;
            public int dySpaceAfter;
            public int dyLineSpacing;
            public short sStyle;
            public byte bLineSpacingRule;
            public byte bOutlineLevel;
            public short wShadingWeight;
            public short wShadingStyle;
            public short wNumberingStart;
            public short wNumberingStyle;
            public short wNumberingTab;
            public short wBorderSpace;
            public short wBorderWidth;
            public short wBorders;
        }

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref PARAFORMAT2 lParam);

        /// <summary>  
        /// 设置行距  
        /// </summary>  
        /// <param name="ctl">控件</param>  
        /// <param name="dyLineSpacing">间距</param>  
        public static void SetLineSpace(Control ctl, int dyLineSpacing)
        {
            PARAFORMAT2 fmt = new PARAFORMAT2();
            fmt.cbSize = Marshal.SizeOf(fmt);
            fmt.bLineSpacingRule = 4;// bLineSpacingRule;  
            fmt.dyLineSpacing = dyLineSpacing;
            fmt.dwMask = PFM_LINESPACING;
            try
            {
                SendMessage(new HandleRef(ctl, ctl.Handle), EM_SETPARAFORMAT, 0, ref fmt);
            }
            catch
            {

            }
        }  
    }
}
