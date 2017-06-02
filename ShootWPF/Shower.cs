using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

namespace ShootGame
{
    public class Shower
    {
        public Role Role { get; set; }
        public FrameworkElement Show { get; set; }

        DispatcherTimer timer;
        public DispatcherTimer Timer
        {
            get { return timer; }
        }

        protected Canvas Canv
        {
            get { return Show.Parent as Canvas; }
        }

        public Shower(Role role, FrameworkElement show)
        {
            Role = role;
            Show = show;
            Canvas.SetLeft(show, role.X);
            Canvas.SetTop(show, role.Y);
            role.OnDestroyed += role_OnDestroyed;
        }

        public Shower(Role role, FrameworkElement show, DispatcherTimer timer)
        {
            Role = role;
            Show = show;
            Canvas.SetLeft(show, role.X);
            Canvas.SetTop(show, role.Y);
            role.OnDestroyed += role_OnDestroyed;
            this.timer = timer;
            timer.Tick += timer_Tick;
        }

        protected virtual void role_OnDestroyed(object sender, EventArgs e)
        {
            timer.Tick -= timer_Tick;
            if (Canv != null)
                Canv.Children.Remove(Show);
        }

        protected virtual void timer_Tick(object sender, EventArgs e)
        {
            if (Canv == null)
                return;
            Role.Move();
            Canvas.SetLeft(Show, Role.X);
            Canvas.SetTop(Show, Role.Y);
        }
    }
    public class BulletShower : Shower
    {
        public BulletShower(Bullet bullet, Shape shape, DispatcherTimer timer)
            : base(bullet, shape, timer)
        {
            shape.Width = bullet.Width;
            shape.Height = bullet.Height;
        }

        protected override void timer_Tick(object sender, EventArgs e)
        {
            base.timer_Tick(sender, e);
            if (Canv == null)
                return;
            if (Role.X > Canv.ActualWidth || Role.Y > Canv.ActualHeight)
            {
                Role.Destroy();
                return;
            }
        }
    }
    public class PumpkinShower : Shower
    {

        public PumpkinShower(Pumpkin pumpkin, string imgPath,DispatcherTimer timer)
            : base(pumpkin, new StackPanel(),timer)
        {
            Image img = new Image() { Source = new BitmapImage(new Uri(imgPath, UriKind.Relative)) };
            img.Width = pumpkin.Width;
            img.Height = pumpkin.Height;
            StackPanel spanel = (StackPanel)Show;
            spanel.HorizontalAlignment = HorizontalAlignment.Left;
            spanel.Children.Add(img);
           
        }

        protected override void timer_Tick(object sender, EventArgs e)
        {
            if (Canv == null)
                return;
            base.timer_Tick(sender, e);
            if (Canvas.GetTop(Show) < 0) { Role.Destroy(); }

        }

    }
}
