using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IriamCommentReader
{
    public partial class AreaSelector : Form
    {
        private Point startPoint;
        private Rectangle selectedArea;
        private bool isDragging = false;

        public AreaSelector()
        {
            InitializeComponent();

        }

        private void AreaSelector_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                startPoint = e.Location;
            }
        }

        private void AreaSelector_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int x = Math.Min(startPoint.X, e.X);
                int y = Math.Min(startPoint.Y, e.Y);
                int width = Math.Abs(startPoint.X - e.X);
                int height = Math.Abs(startPoint.Y - e.Y);

                selectedArea = new Rectangle(x, y, width, height);
                this.Invalidate(); // 再描画
            }
        }

        private void AreaSelector_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                // 選択された領域をここで使用
                Console.WriteLine($"選択された領域: {selectedArea}");

                // フォームを閉じる
                this.Close();
            }
        }


        private void AreaSelector_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close(); // Escキーでキャンセルして閉じる
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (isDragging)
            {
                // 選択領域の描画
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(pen, selectedArea);
                }
            }
        }


        public static Rectangle GetSelectedArea()
        {
            using (var selector = new AreaSelector())
            {
                selector.ShowDialog();
                return selector.selectedArea;
            }
        }

        private void AreaSelector_Load(object sender, EventArgs e)
        {

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.BackColor = Color.Blue; // 半透明の青
            this.Opacity = 0.5;  // 透明度を設定
            this.DoubleBuffered = true; // 描画のちらつき防止

            this.MouseDown += AreaSelector_MouseDown;
            this.MouseMove += AreaSelector_MouseMove;
            this.MouseUp += AreaSelector_MouseUp;
            this.KeyDown += AreaSelector_KeyDown; // Escキーでキャンセル
        }
    }
}
