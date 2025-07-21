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

        private void AreaSelector_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Bounds = SystemInformation.VirtualScreen; // Use the entire virtual screen
            this.BackColor = Color.Blue;
            this.Opacity = 0.5;
            this.DoubleBuffered = true;

            this.MouseDown += AreaSelector_MouseDown;
            this.MouseMove += AreaSelector_MouseMove;
            this.MouseUp += AreaSelector_MouseUp;
            this.KeyDown += AreaSelector_KeyDown;
        }

        private void AreaSelector_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                // Convert the starting point to screen coordinates
                startPoint = this.PointToScreen(e.Location);
            }
        }

        private void AreaSelector_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // Convert the current point to screen coordinates
                Point currentPoint = this.PointToScreen(e.Location);

                int x = Math.Min(startPoint.X, currentPoint.X);
                int y = Math.Min(startPoint.Y, currentPoint.Y);
                int width = Math.Abs(startPoint.X - currentPoint.X);
                int height = Math.Abs(startPoint.Y - currentPoint.Y);

                // The selectedArea is now in screen coordinates
                selectedArea = new Rectangle(x, y, width, height);
                this.Invalidate(); // Request a repaint
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
                this.Close(); // Cancel and close on Esc
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw borders for all screens
            foreach (var screen in Screen.AllScreens)
            {
                using (Pen pen = new Pen(Color.LightGreen, 3))
                {
                    // Convert screen bounds to client coordinates for drawing
                    var rect = this.RectangleToClient(screen.Bounds);
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }

            if (isDragging)
            {
                // Draw the selected area
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    // Convert the selected area (in screen coords) to client coords for drawing
                    e.Graphics.DrawRectangle(pen, this.RectangleToClient(selectedArea));
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
    }
}