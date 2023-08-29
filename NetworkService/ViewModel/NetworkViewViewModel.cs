using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetworkService.ViewModel
{
    public class NetworkViewViewModel : BindableBase
    {
        public static int monitor = 0;
        private ListView lv;
        public BindingList<Agriculture> Items { get; set; }
        public MyICommand<ListView> MLBUCommand { get; set; }
        public MyICommand<Agriculture> SCCommand { get; set; }
        public MyICommand<Canvas> DCommand { get; set; }
        public MyICommand<Canvas> FreeCommand { get; set; }
        public MyICommand<Canvas> LCommand { get; set; }
        public MyICommand<ListView> LLWCommand { get; set; }
        public MyICommand<Grid> AddGenerator { get; set; }

        Dictionary<int, Agriculture> temp = new Dictionary<int, Agriculture>();
        public static Agriculture draggedItem = null;
        private bool dragging = false;
        private static bool exists = false;
        private int selectedIndex = 0;
        int count = 0;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public NetworkViewViewModel()
        {
            foreach (Agriculture ob in DB.Generators)
            {
                temp.Add(ob.Id, ob);
                count++;
            }
            Items = new BindingList<Agriculture>();
            foreach (var item in DB.Generators)
            {
                exists = false;
                foreach (var ex in DB.CanvasObjects.Values)
                {
                    if (ex.Id == item.Id)
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists == false)
                    Items.Add(new Agriculture(item));
            }
            MLBUCommand = new MyICommand<ListView>(OnMLBU);
            SCCommand = new MyICommand<Agriculture>(SelectionChange);
            DCommand = new MyICommand<Canvas>(OnDrop);
            FreeCommand = new MyICommand<Canvas>(OnFree);
            LCommand = new MyICommand<Canvas>(OnLoad);
            LLWCommand = new MyICommand<ListView>(OnLLW);
            AddGenerator = new MyICommand<Grid>(OnAdd, CanAdd);
        }

        public void OnLLW(ListView listview)
        {
            lv = listview;
        }

        public void OnLoad(Canvas c)
        {
            if (DB.CanvasObjects.ContainsKey(c.Name))
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(DB.CanvasObjects[c.Name].Type.Photo);
                logo.EndInit();
                c.Background = new ImageBrush(logo);
                c.Resources.Add("taken", true);
                monitor++;
                CheckValue(c);
            }
        }

        public void OnFree(Canvas c)
        {
            try
            {
                if (c.Resources["taken"] != null)
                {
                    c.Background = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                    ((Border)c.Children[0]).BorderBrush = Brushes.Transparent;

                    foreach (var item in DB.Generators)
                    {
                        if (!Items.Contains(item) && DB.CanvasObjects[c.Name].Id == item.Id)
                        {
                            Items.Add(new Agriculture(item));
                        }
                    }
                    c.Resources.Remove("taken");
                    DB.CanvasObjects.Remove(c.Name);
                }
                monitor--;
            }
            catch (Exception) { }
            AddGenerator.RaiseCanExecuteChanged();
        }

        public void OnDrop(Canvas c)
        {
            if (draggedItem != null)
            {
                if (c.Resources["taken"] == null)
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(draggedItem.Type.Photo);
                    logo.EndInit();
                    c.Background = new ImageBrush(logo);
                    DB.CanvasObjects[c.Name] = draggedItem;
                    c.Resources.Add("taken", true);
                    Items.Remove(Items.Single(x => x.Id == draggedItem.Id));
                    SelectedIndex = 1;
                    monitor++;
                    CheckValue(c);
                }
                dragging = false;
            }
            AddGenerator.RaiseCanExecuteChanged();
        }

        public void OnMLBU(ListView lw)
        {
            draggedItem = null;
            lw.SelectedItem = null;
            dragging = false;
        }

        public void SelectionChange(Agriculture o)
        {
            if (!dragging)
            {
                dragging = true;
                draggedItem = new Agriculture(o);
                DragDrop.DoDragDrop(lv, draggedItem, DragDropEffects.Move);
            }
        }



        public void OnAdd(Grid allCanvas)
        {
            int i = 0;
            int len = Items.Count();
            foreach (Canvas c in allCanvas.Children)
            {
                if (c.Resources["taken"] == null)
                {
                    Agriculture v = new Agriculture(Items[i]);
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(v.Type.Photo);
                    logo.EndInit();
                    c.Background = new ImageBrush(logo);
                    DB.CanvasObjects[c.Name] = v;
                    c.Resources.Add("taken", true);
                    SelectedIndex = 0;
                    monitor++;
                    CheckValue(c);
                    i++;
                    if (i == len)
                    {
                        break;
                    }
                }
            }
            Items = null;
            Items = new BindingList<Agriculture>();
            AddGenerator.RaiseCanExecuteChanged();
        }

        public bool CanAdd(Grid grid)
        {
            if (Items.Count > 0 && Items.Count < 15)
                return true;
            return false;
        }

        private void CheckValue(Canvas c)
        {

            foreach (Agriculture ob in DB.Generators)
            {
                temp[ob.Id] = ob;
            }
            Task.Delay(1000).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (DB.CanvasObjects.Count != 0)
                    {
                        if (DB.CanvasObjects.ContainsKey(c.Name))
                        {
                            if (temp[DB.CanvasObjects[c.Name].Id].Value < 17)
                            {
                                ((Border)c.Children[0]).BorderBrush = Brushes.Red;
                            }
                            else
                            {
                                ((Border)c.Children[0]).BorderBrush = Brushes.Transparent;
                            }
                        }
                        else
                        {
                            ((Border)c.Children[0]).BorderBrush = Brushes.Transparent;
                        }
                    }
                });
                CheckValue(c);
            });

        }
    }
}
