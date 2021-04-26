using KyuBase.Integrations;
using KyuWIN.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KyuWIN.WinComponents
{
    public class FHandle : KyuBase.Objects.FHandle
    {
        public Form form;

        public FHandle(int height, int width, Point offSets, string title, bool sizable) : base(height, width, offSets, title, sizable)
        {
            form = new Form() { Width = width, Height = height, Text = title, MaximizeBox = sizable, FormBorderStyle = sizable ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle };
            xOffset = (int)form.ClientRectangle.Width;
            yOffset = (int)form.ClientRectangle.Height;

            fThr = new System.Threading.Thread(() => formRun());
            refresher = new System.Threading.Thread(refresh);

            fThr.Start();

            form.FormClosing += Form_FormClosing;
        }

        #region EventPassThrough
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
            => formClosing(sender, e.ToKyu());

        private void Pd_Paint(object sender, PaintEventArgs e)
            => formPaint(sender, e.ToKyu());

        private void Form_KeyPress(object sender, KeyPressEventArgs e)
            => formKeyPress(sender, e.ToKyu());

        private void FHandle_MouseMove(object sender, MouseEventArgs e)
            => formMouseMove(sender, e.ToKyu());

        private void FHandle_MouseClick(object sender, MouseEventArgs e)
            => formMouseClick(sender, e.ToKyu());

        private void FHandle_Paint(object sender, PaintEventArgs e)
                => handlePaint(sender, e.ToKyu());
        #endregion

        public override void formRun()
        {
            DrawPanel pd = new DrawPanel() { Width = this.Width, Height = this.Height, Dock = DockStyle.Fill };
            form.Controls.Add(pd);
            pd.Paint += Pd_Paint;
            form.Controls[0].Paint += FHandle_Paint; ;
            form.Controls[0].MouseClick += FHandle_MouseClick;
            form.Controls[0].MouseMove += FHandle_MouseMove;
            form.Controls[0].Refresh();
            refresher.Start();
            form.KeyPress += Form_KeyPress;
            form.ShowDialog();
        }

        public override void refresh()
        {
            while (true)
            {
                try
                {
                    form.Controls[0].Invoke((MethodInvoker)(() => form.Controls[0].Refresh()));
                    Thread.Sleep(140);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
