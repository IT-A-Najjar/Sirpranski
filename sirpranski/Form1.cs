using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sirpranski
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void draw_triangle(Graphics graphic,PointF top,PointF left,PointF right)
        {
            Pen pen = new Pen(Brushes.Black);
            pen.Width = 1;

            graphic.DrawPolygon(pen, new PointF[] { top, left, right });
        }

        void call_s_triangel(Graphics graphic,PointF top,PointF left,PointF right,int level)
        {
            if (level == 0)
            {
                draw_triangle(graphic, top, left, right);
            }
            else
            {
                PointF mid_left = new PointF((top.X + left.X) / 2, (top.Y + left.Y) / 2);
                PointF mid_right = new PointF((top.X + right.X) / 2, (top.Y + right.Y) / 2);
                PointF mid_bottom = new PointF((right.X + left.X) / 2, (right.Y + left.Y) / 2);

                call_s_triangel(graphic, top, mid_left, mid_right, level - 1);
                call_s_triangel(graphic, mid_left, left, mid_bottom, level - 1);
                call_s_triangel(graphic, mid_right, mid_bottom, right, level - 1);
            }
        }
        PointF[] find_pentagon_points_general(PointF center,float radius,int number)
        {
            PointF[] points = new PointF[number];
            double current_angel = -Math.PI / 2;
            double move = (2 * Math.PI) / number;

            for(int i = 0; i < number; i++)
            {
                points[i] = new PointF(center.X + (float)(radius * Math.Cos(current_angel)), center.Y + (float)(radius * Math.Sin(current_angel)));
                current_angel += move;
            }
            return points;
        }

        void call_s_polygon(Graphics graphic, PointF center, float radius, int level)
        {
            PointF[] points;
            float scale = tb_scale.Value / 200.0f;
            int polygons = tb_poly.Value;

            if (level == 0)
            {
                points = find_pentagon_points_general(center, radius, polygons);
                graphic.FillPolygon(Brushes.Green, points);
            }
            else
            {
                float subtract_radius = radius - (radius * scale);
                points = find_pentagon_points_general(center, subtract_radius, polygons);
                for (int i = 0; i < polygons; i++)
                {
                    call_s_polygon(graphic, points[i], (radius * scale), level - 1);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Bitmap bitmap = new Bitmap(canves.ClientSize.Width, canves.ClientSize.Height);
            canves.Image = bitmap;

            using (Graphics graphic = Graphics.FromImage(bitmap))
            {
                graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphic.Clear(canves.BackColor);
                int level = Convert.ToInt32(textBox1.Text);

                if(comboBox1.Text== "sirpranski triangle")
                {
                    PointF top = new PointF((canves.Width / 2f), 15);
                    PointF left = new PointF(15, canves.Height - 15);
                    PointF right = new PointF(canves.Width - 15, canves.Height - 15);
                     
                    call_s_triangel(graphic, top, left, right, level);
                }
                else if(comboBox1.Text== "sirpranski polygon")
                {
                    PointF center = new PointF(canves.ClientSize.Width / 2, canves.ClientSize.Height / 2);

                    float radius = Math.Min(center.X, center.Y) - 20;
                    call_s_polygon(graphic, center, radius, level);
                }
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tb_poly.Enabled = false;
            tb_scale.Enabled = false;
            if(comboBox1.Text== "sirpranski polygon")
            {
                tb_poly.Enabled = true;
                tb_scale.Enabled = true;
            }
        }
    }
}
