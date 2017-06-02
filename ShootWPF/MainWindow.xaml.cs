using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Media;
using ShootGame;

namespace ShootWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer disTimer;
        int countSecond;
        Game game;
        List<AttackShower> attackerShowers;
        List<DefendShower> defenderShowers;
        List<PumpkinShower> pumpkinShowers;

        int iSel = 0;
        int goldNum;
        double rowHeight;
        double colWidth;
        MediaPlayer player = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            game = new Game();
            attackerShowers = new List<AttackShower>();
            defenderShowers = new List<DefendShower>();
            pumpkinShowers = new List<PumpkinShower>();
            rowHeight = canvas1.ActualHeight / 5;
            colWidth = canvas1.ActualWidth / 9;
            disTimer = new DispatcherTimer();
            disTimer.Interval = TimeSpan.FromMilliseconds(1000);
            disTimer.Tick += new EventHandler(disTimer_Tick);
            disTimer.Start();
            countSecond = 120;
            goldNum = 180;
            time.Content = countSecond.ToString();
            GoldNum.Content = goldNum.ToString();
            imghag.ToolTip = "女巫";
            imgwarrior.ToolTip = "吸血鬼";
            
            player.Open(new Uri(Environment.CurrentDirectory + @"\Resources\bgMusic.wav"));
            player.Play();

        }
        private void canvas1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition(canvas1);
            int rowNum = (int)(pt.Y / rowHeight);
            int colNum = (int)(pt.X / colWidth);
            if (pt.X > canvas1.ActualWidth || pt.Y > canvas1.ActualHeight)
                return;
            if (iSel == 1)
            {
                Hag hag = new Hag() { X = colNum * colWidth + colWidth / 3, Y = rowNum * rowHeight };
                HagShower hagSh = new HagShower(hag, "/Resources/hag.png", new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100) });
                if (!game.AddDefender(hag))
                    return;
                canvas1.Children.Add(hagSh.Show);
                defenderShowers.Add(hagSh);
                iSel = 0;
                hagSh.Timer.Start();
                this.Cursor = Cursors.Arrow;
                goldNum -= 50;
            }
            else if (iSel == 2)
            {
                Warrior warrior = new Warrior() { X = colNum * colWidth + colWidth / 3, Y = rowNum * rowHeight };
                WarriorShower warSh = new WarriorShower(warrior, "/Resources/warrior.png", new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(120) });
                if (!game.AddDefender(warrior))
                    return;
                canvas1.Children.Add(warSh.Show);
                defenderShowers.Add(warSh);
                iSel = 0;
                warSh.Timer.Start();
                this.Cursor = Cursors.Arrow;
                goldNum -= 40;
            }
            else if (iSel == 0 && game.isClickPumpkin(pt))
            {
                SoundPlayer soundPlayer =new SoundPlayer(System.Environment.CurrentDirectory + @"\Resources\getPumpkin.wav");
                soundPlayer.Play();
                goldNum += 30;
            }
            GoldNum.Content = goldNum.ToString();
        }
        private void hag_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (goldNum < 50)
            {
                imghag.ToolTip = "金币不足";
                return;
            }
            iSel = 1;
            this.Cursor = Cursors.Hand;
        }
        private void warrior_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (goldNum < 40)
            {
                imghag.ToolTip = "金币不足";
                return;
            }
            iSel = 2;
            this.Cursor = Cursors.Hand;
        }


        private void disTimer_Tick(object sender, EventArgs e)
        {
            if (countSecond == 0)
            {
                disTimer.Stop();
                canvas1.Children.Clear();
                attackerShowers.Clear();
                defenderShowers.Clear();
                game.Clear();
                SoundPlayer soundPlayer = new SoundPlayer(System.Environment.CurrentDirectory + @"\Resources\gameOver.wav");
                soundPlayer.Play();
                MessageBox.Show("闯关成功！");
                countSecond = 120;
                goldNum = 180;
                time.Content = countSecond.ToString();
                GoldNum.Content = goldNum.ToString();
            }
            else if (countSecond > 0 && game.Isfalied())
            {
                disTimer.Stop();
                canvas1.Children.Clear();
                attackerShowers.Clear();
                defenderShowers.Clear();
                game.Clear();
                SoundPlayer soundPlayer = new SoundPlayer(System.Environment.CurrentDirectory + @"\Resources\gameOver.wav");
                soundPlayer.Play();
                MessageBox.Show("闯关失败！");
                countSecond = 120;
                goldNum = 180;
                time.Content = countSecond.ToString();
                GoldNum.Content = goldNum.ToString();
            }
            else
            {
                Random rd = new Random();
                if (rd.Next() % 2 == 0)
                {
                    int j = rd.Next() % 5;
                    if (rd.Next() % 2 == 0)
                    {
                        Ghost ghost = new Ghost { X = colWidth * 9, Y = j * rowHeight + rowHeight / 7 };
                        GhostShower ghSh = new GhostShower(ghost, "/Resources/ghost.png", new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(150) });
                        attackerShowers.Add(ghSh);
                        canvas1.Children.Add(ghSh.Show);
                        SoundPlayer soundPlayer = new SoundPlayer(System.Environment.CurrentDirectory + @"\Resources\putDefend.wav");
                        soundPlayer.Play();
                        game.AddAttacker(ghost);
                        ghSh.Timer.Start();
                    }
                    else
                    {
                        Devil devil = new Devil { X = colWidth * 9, Y = j * rowHeight };
                        DevilShower dvSh = new DevilShower(devil, "/Resources/devil.png", new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(150) });
                        attackerShowers.Add(dvSh);
                        canvas1.Children.Add(dvSh.Show);
                        SoundPlayer soundPlayer = new SoundPlayer(System.Environment.CurrentDirectory + @"\Resources\putDefend.wav");
                        soundPlayer.Play();
                        game.AddAttacker(devil);
                        dvSh.Timer.Start();
                    }
                }
                if (rd.Next() % 6 == 0)
                {
                    Pumpkin pum = new Pumpkin { X = rd.Next() % (int)(colWidth * 8), Y = rowHeight * 5 };
                    PumpkinShower puSh = new PumpkinShower(pum, "/Resources/pumpkin.png", new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(10) });
                    if (!game.AddPumpkin(pum))
                        return;
                    pumpkinShowers.Add(puSh);
                    canvas1.Children.Add(puSh.Show);
                    puSh.Timer.Start();
                    
                }
            }
            time.Content = countSecond.ToString();
            countSecond--;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = defenderShowers.Count - 1; i >= 0; i--)
                defenderShowers[i].Timer.Stop();
            for (int i = attackerShowers.Count - 1; i >= 0; i--)
                attackerShowers[i].Timer.Stop();
            for (int i = pumpkinShowers.Count - 1; i >= 0; i--)
                pumpkinShowers[i].Timer.Stop();
            disTimer.Stop();
        }
        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = defenderShowers.Count - 1; i >= 0; i--)
                defenderShowers[i].Timer.Start();
            for (int i = attackerShowers.Count - 1; i >= 0; i--)
                attackerShowers[i].Timer.Start();
            for (int i = pumpkinShowers.Count - 1; i >= 0; i--)
                pumpkinShowers[i].Timer.Start();
            disTimer.Start();
            if (countSecond < 0)
            {
                countSecond = 120;
                goldNum = 180;
                time.Content = countSecond.ToString();
                GoldNum.Content = goldNum.ToString();
            }
        }
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            game.Clear();
            for (int i = defenderShowers.Count - 1; i >= 0; i--)
                canvas1.Children.Remove(defenderShowers[i].Show);
            for (int i = attackerShowers.Count - 1; i >= 0; i--)
                canvas1.Children.Remove(attackerShowers[i].Show);
            for (int i = pumpkinShowers.Count - 1; i >= 0; i--)
                canvas1.Children.Remove(pumpkinShowers[i].Show);
            defenderShowers.Clear();
            attackerShowers.Clear();
            pumpkinShowers.Clear();
            countSecond = 120;
            goldNum = 180;
            time.Content = countSecond.ToString();
            GoldNum.Content = goldNum.ToString();
        }
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            game.Clear();
            disTimer.Stop();
            this.Close();
            Application.Current.MainWindow.Show();

        }

    }
}
