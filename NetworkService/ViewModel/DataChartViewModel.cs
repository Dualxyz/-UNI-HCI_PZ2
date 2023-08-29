using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkService.ViewModel
{
    public class DataChartViewModel : BindableBase
    {
        public Dictionary<int, Agriculture> ComboBoxDataAgriculture { get; set; }  
        public static Graph ElementHeights { get; set; } = new Graph();
        private static int generatorChoice;


        public DataChartViewModel()
        {
            ComboBoxDataAgriculture = new Dictionary<int, Agriculture>();
            foreach (Agriculture a in DB.Generators)
            {
                if (!ComboBoxDataAgriculture.ContainsKey(a.Id))
                {
                    ComboBoxDataAgriculture.Add(a.Id, a);
                }
            }
        }


        public static int AgricultureChoice
        {
            get { return generatorChoice; }
            set
            {
                generatorChoice = value;
                List<int> values = new List<int>();
                List<DateTime> dates = new List<DateTime>();
                string[] lines = File.ReadAllLines("LogFile.txt");
                List<String> l = lines.ToList();
                l.Reverse();
                foreach (string s in l)
                {
                    DateTime dt = DateTime.Parse($"{s.Split(':', '|')[5]}:{s.Split(':', '|')[6]}:{s.Split(':', '|')[7]}".Trim());
                    int id = int.Parse(s.Split(':', '|')[1]);
                    if (id == AgricultureChoice)
                    {
                        values.Add(int.Parse(s.Split(':', '|')[3]));
                        dates.Add(dt);
                    }

                    if (dates.Count == 5)
                        break;
                }

                int duzina = values.Count();
                if (duzina >= 1)
                {
                    if (values[0] > 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height1 = ViewModel.DataChartViewModel.CalcHg(values[0]);
                        ViewModel.DataChartViewModel.ElementHeights.Fill1 = "Blue";
                    }
                    else if (values[0] < 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height1 = ViewModel.DataChartViewModel.CalcHg(values[0]);
                        ViewModel.DataChartViewModel.ElementHeights.Fill1 = "Red";
                    }
                }
                else
                {
                    ViewModel.DataChartViewModel.ElementHeights.Height1 = -1;
                }

                if (duzina >= 2)
                {
                    if (values[1] > 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height2 = ViewModel.DataChartViewModel.CalcHg(values[1]);
                        ViewModel.DataChartViewModel.ElementHeights.Fill2 = "Blue";
                    }
                    else if (values[1] < 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height2 = ViewModel.DataChartViewModel.CalcHg(values[1]);
                        ViewModel.DataChartViewModel.ElementHeights.Fill2 = "Red";
                    }
                }
                else
                {
                    ViewModel.DataChartViewModel.ElementHeights.Height2 = -1;
                }

                if (duzina >= 3)
                {
                    if (values[2] > 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height3 = ViewModel.DataChartViewModel.CalcHg(values[2]);
                        ViewModel.DataChartViewModel.ElementHeights.Fill3 = "Blue";
                    }
                    else if (values[2] < 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height3 = ViewModel.DataChartViewModel.CalcHg(values[2]);
                        ViewModel.DataChartViewModel.ElementHeights.Fill3 = "Red";
                    }
                }
                else
                {
                    ViewModel.DataChartViewModel.ElementHeights.Height3 = -1;
                }

                if (duzina >= 4)
                {
                    if (values[3] > 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height4 = ViewModel.DataChartViewModel.CalcHg(values[3]);
                        ViewModel.DataChartViewModel.ElementHeights.Fill4 = "Blue";
                    }
                    else if (values[3] < 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height4 = ViewModel.DataChartViewModel.CalcHg(values[3]);
                        ViewModel.DataChartViewModel.ElementHeights.Fill4 = "Red";
                    }
                }
                else
                {
                    ViewModel.DataChartViewModel.ElementHeights.Height4 = -1;
                }

                if (duzina >= 5)
                {
                    if (values[4] > 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height5 = ViewModel.DataChartViewModel.CalcHg(values[4]);
                        ViewModel.DataChartViewModel.ElementHeights.Fill5 = "Blue";
                    }
                    else if (values[4] < 17)
                    {
                        ViewModel.DataChartViewModel.ElementHeights.Height5 = ViewModel.DataChartViewModel.CalcHg(values[4]);
                        ViewModel.DataChartViewModel.ElementHeights.Fill5 = "Red";
                    }
                }
                else
                {
                    ViewModel.DataChartViewModel.ElementHeights.Height5 = -1;
                }
            }
        }

        public static double CalcHg(double value)
        {
            return value * 6.57;
        }
    }
}
