using NetworkService.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkService.ViewModel
{
    public class NetworkDataViewModel : BindableBase
    {
        private DB db = new DB();
        public List<Agriculture> AgrRes = new List<Agriculture>();
        public ObservableCollection<Agriculture> AgrOC { get; set; } = new ObservableCollection<Agriculture>();
        public Dictionary<int, Agriculture> Reserve { get; set; } = new Dictionary<int, Agriculture>();
        public List<string> CBData { get; set; }     
        public List<string> ComboBoxDataAgriculture { get; set; }   
        public MyICommand AddCommand { get; set; }      
        public MyICommand DeleteCommand { get; set; }


        bool addUndo, deleteUndo = false;
        public MyICommand SearchCommand { get; set; }
        public MyICommand ResetCommand { get; set; }
        public MyICommand NameSearchCommand { get; set; }
        public MyICommand TypeSearchCommand { get; set; }
        public ObservableCollection<Agriculture> Agries { get; set; } = new ObservableCollection<Agriculture>();
        public static MyICommand<string> UndoCommand { get; set; }
        private string idSearch;
        private string typeText;
        private string id;
        private string name;
        Stack Added = new Stack();
        Stack Deleted = new Stack();
        Stack Stektf = new Stack();

        private string searchValueText;
        private int nameOrType = -1;

        private int index = -1;
        private string path;
        private int clickSearch = 0;
        private string valueName;
        private string valueId;

        public NetworkDataViewModel()
        {
            if (Agries.Count() > 0)
            {
                Agries.Clear();
            }

            foreach (var i in DB.Generators)
            {
                Agries.Add(i);
            }

            CBData = new List<string>();
            ComboBoxDataAgriculture = new List<string> { "industrial", "wheat" };
            foreach (Agriculture a in Agries)
            {
                if (!Reserve.ContainsKey(a.Id))
                {
                    Reserve.Add(a.Id, a);
                }
            }


            NameCheck = "True";
            ResetCommand = new MyICommand(OnReset, CanReset);
            NameOrType = 0;
            UndoCommand = new MyICommand<string>(OnUndo);
            AddCommand = new MyICommand(OnAdd, CanAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            SearchCommand = new MyICommand(OnSearch, CanSearch);
            NameSearchCommand = new MyICommand(OnName);
            TypeSearchCommand = new MyICommand(OnType);
        }

        public DB DB
        { get => db; set => db = value; }


        private void OnUndo(string s)
        {
            if (Stektf.Count > 0)
            {
                if ((bool)Stektf.Peek())
                {
                    Stektf.Pop();
                    Agriculture a = new Agriculture();
                    a = (Agriculture)Added.Pop();
                    Agriculture zaBrisanje = new Agriculture();
                    int j = -1;
                    if (a != null)  //Zadnje je dodato sada ga brise
                    {
                        if (Agries.Contains(a))
                        {
                            deleteUndo = true;
                            Deleted.Push(DB.Generators.Last());
                            DB.Generators.Remove(a);
                            Agries.Remove(a);
                        }
                    }
                    addUndo = false;
                }
                else
                {
                    Stektf.Pop();
                    Agriculture a = new Agriculture();
                    a = (Agriculture)Deleted.Pop();
                    DB.Generators.Add(a);
                    Agries.Add(a);
                }
            }
        }

        public int NameOrType
        {
            get { return nameOrType; }
            set
            {
                nameOrType = value;
            }
        }
        public string IdSearch
        {
            get { return idSearch; }
            set
            {
                idSearch = value;
                OnPropertyChanged("IdSearch");
                SearchCommand.RaiseCanExecuteChanged();
            }
        }


        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
                AddCommand.RaiseCanExecuteChanged();
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
                AddCommand.RaiseCanExecuteChanged();
            }
        }
        public string TypeText
        {
            get { return typeText; }
            set
            {
                typeText = value;
                OnPropertyChanged("TypeText");
                Path = Environment.CurrentDirectory;
                Path = Path.Remove(Path.Length - 10, 10);
                Path += @"\Photos\" + value + ".jpg";
                AddCommand.RaiseCanExecuteChanged();
            }
        }
        public string SearchValueText
        {
            get { return searchValueText; }
            set
            {
                searchValueText = value;
                OnPropertyChanged("SearchValueText");
                SearchCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanSearch()
        {
            if ((SearchValueText != null && SearchValueText != "") && ClickSearch != 1)
            {
                return true;
            }
            else
            {
                
                return false;
            }


        }

        private bool CanReset()
        {
            if (ClickSearch == 1)
                return true;
            return false;
        }

        private void OnReset()
        {
            Agries.Clear();
            foreach (Agriculture v in Reserve.Values)
            {
                Agries.Add(v);
            }
            AgrOC.Clear();

            ClickSearch = 0;
        }


        private void OnSearch()
        {
            if (ClickSearch == 0)
            {
                foreach (Agriculture a in Agries)
                {
                    AgrOC.Add(a);
                }

                if (NameOrType == 0) //Name pretraga
                {
                    foreach (var item in Agries)
                    {
                        if (item.Name.Contains(SearchValueText))
                        {
                            AgrRes.Add(item);
                        }
                    }

                }
                else if (NameOrType == 1)
                {
                    foreach (var item in Agries)
                    {
                        if (item.Type.Name.Contains(SearchValueText))
                        {
                            AgrRes.Add(item);
                        }
                    }

                }
                Agries.Clear();
                foreach (Agriculture v in AgrRes)
                {
                    Agries.Add(v);
                }
                AgrRes.Clear();
                ClickSearch = 1;
                SearchValueText = "";
                ResetCommand.RaiseCanExecuteChanged();
            }
        }
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        public int ClickSearch
        {
            get { return clickSearch; }
            set
            {
                clickSearch = value;

            }
        }
 

        public string NameCheck
        {
            get { return valueName; }
            set
            {
                valueName = value;
                OnPropertyChanged("NameCheck");
            }
        }

        public string IdCheck
        {
            get { return valueId; }
            set
            {
                valueId = value;
                OnPropertyChanged("IdCheck");
            }
        }

        private bool CanDelete()
        {
            return index >= 0;
        }

        private void OnDelete()
        {
            bool valid = true;
            foreach (Agriculture v in DB.CanvasObjects.Values)
            {
                if (v.Id == Agries[index].Id)
                {
                    valid = false;
                    MessageBox.Show("Entity is on monitoring", "Err", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }

            if (valid)
            {
                Stektf.Push(false);

                int i = index;
                Deleted.Push(Agries[i]);
                Agries.RemoveAt(i);
                DB.Generators.RemoveAt(i);
            }

        }
        public int idInt;
        private bool CanAdd()
        {
            if (TypeText != null && Name != null && Name != "")
            {
                return true;
            }
            return false;

        }



        private void OnAdd()
        {
            if (Int32.TryParse(Id, out idInt))
            {
                bool postoji = false;
                if (DB.Generators.Count != 0)
                {
                    foreach (Agriculture v in DB.Generators)
                    {
                        if (idInt == v.Id)
                        {
                            MessageBox.Show("Not an unique ID", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            postoji = true;
                        }
                    }
                }

                if (!postoji)
                {
                    Agriculture i = new Agriculture();
                    i.Id = idInt;
                    i.Name = Name;
                    i.Type.Name = TypeText;
                    string pathImg = Environment.CurrentDirectory;
                    pathImg = pathImg.Remove(pathImg.Length - 10, 10);
                    pathImg += @"\Photos\" + TypeText + ".jpg";
                    i.Type.Photo = pathImg;
                    Added.Push(i);
                    Stektf.Push(true);
                    Agries.Add(i);
                    DB.Generators.Add(i);
                    foreach (Agriculture a in DB.Generators)
                    {
                        if (!Reserve.ContainsKey(a.Id))
                        {
                            Reserve.Add(a.Id, a);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("ID is whole num", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void OnName()
        {
            NameOrType = 0;
        }
        public void OnType()
        {
            NameOrType = 1;
        }


    }
}
