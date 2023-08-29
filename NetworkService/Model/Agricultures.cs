using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class DB
    {
        public static ObservableCollection<Agriculture> Generators { get; set; } = new ObservableCollection<Agriculture>();
        public static Dictionary<string, Agriculture> CanvasObjects { get; set; } = new Dictionary<string, Agriculture>();
    }

    public class Agriculture : BindableBase
    {
        private int id;
        private string name;
        private Type type = new Type();
        private double value;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        public double Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
                int a = ViewModel.DataChartViewModel.AgricultureChoice;
                if (a == this.id)
                {
                    if (value > 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height1 = ViewModel.DataChartViewModel.CalcHg(value);
                        ViewModel.DataChartViewModel.ElementHeights.Fill1 = "Blue";
                    }
                    else if (value < 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height1 = ViewModel.DataChartViewModel.CalcHg(value);
                        ViewModel.DataChartViewModel.ElementHeights.Fill1 = "Red";
                    }
                }
            }
        }



        public Type Type
        {
            get { return type; }
            set
            {
                type.Name = value.Name;
                type.Photo = value.Photo;
            }
        }



        public Agriculture()
        {
        }
        public Agriculture(Agriculture a)
        {
            Id = a.Id;
            Name = a.Name;
            Type = a.type;
            Value = a.Value;
        }
    }
}
