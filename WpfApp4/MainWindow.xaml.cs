using CsvHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace WpfApp4
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Drink> drinks = new List<Drink>();
        List<OrderItem> order = new List<OrderItem>();
        string takeout;
        public MainWindow()
        {
            InitializeComponent();
            AddNewClass1(drinks);
            Displaydrinks(drinks);
        }
        private void Displaydrinks(List<Drink> drinks)
        {
            foreach (Drink d in drinks)
            {
                StackPanel sp = new StackPanel();
                CheckBox cb = new CheckBox();
                //TextBox tb = new TextBox();
                Slider sl = new Slider();
                Label lb = new Label();
                sp.Orientation = Orientation.Horizontal;
                cb.Content = d.Name + d.Size + d.Price;
                cb.Margin = new Thickness(5);
                cb.Width = 200;
                cb.Height = 25;
                sl.Value = 0;
                sl.Width = 100;
                sl.Minimum = 0;
                sl.Maximum = 10;
                sl.TickPlacement = TickPlacement.TopLeft;
                sl.TickFrequency = 1;
                sl.IsSnapToTickEnabled = true;
                lb.Width = 50;
                //tb.Width = 80;
                //tb.Height = 25;
                //tb.TextAlignment = TextAlignment.Right;
                Binding myBinding = new Binding("Value");
                myBinding.Source = sl;
                lb.SetBinding(ContentProperty, myBinding);
                sp.Children.Add(cb);
                sp.Children.Add(sl);
                sp.Children.Add(lb);
                StackPanel_DrinkMenu.Children.Add(sp);
            }
        }
        private void AddNewClass1(List<Drink> myClass)
        {
            //myClass.Add(new Drink() { Name = "甘蔗檸檬青", Size = "大杯", Price = 60 });
            //myClass.Add(new Drink() { Name = "甘蔗檸檬青", Size = "小杯", Price = 50 });
            //myClass.Add(new Drink() { Name = "甘蔗牛奶", Size = "大杯", Price = 80 });
            //myClass.Add(new Drink() { Name = "甘蔗牛奶", Size = "小杯", Price = 60 });
            //myClass.Add(new Drink() { Name = "QQㄋㄟㄋㄟ好喝到靠杯咩噗茶", Size = "大杯", Price = 90 });
            //myClass.Add(new Drink() { Name = "QQㄋㄟㄋㄟ好喝到靠杯咩噗茶", Size = "小杯", Price = 75 });
            //myClass.Add(new Drink() { Name = "靠杯紅茶", Size = "大杯", Price = 25 });
            //myClass.Add(new Drink() { Name = "靠杯青茶", Size = "大杯", Price = 25 });
            //myClass.Add(new Drink() { Name = "靠杯綠茶", Size = "大杯", Price = 25 });
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "CSV檔案|*.csv|文字檔案|*.txt|所有檔案|*.*";
            dialog.DefaultExt = "*.csv";
            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                //string content = File.ReadAllText(path);
                StreamReader sr = new StreamReader(path, Encoding.Default);
                CsvReader csv = new CsvReader(sr, CultureInfo.InvariantCulture);
                csv.Read();
                csv.ReadHeader();
                while (csv.Read() == true)
                {
                    Drink d = new Drink()
                    {
                        Name = csv.GetField("Name"),
                        Size = csv.GetField("Size"),
                        Price
                   = csv.GetField<int>("Price")
                    };
                    myClass.Add(d);
                }
            }
            MessageBox.Show($"總共有{myClass.Count}筆紀錄");
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rd = sender as RadioButton;
            if (rd.IsChecked == true)
            {
                takeout = rd.Content.ToString();
            }
        }
        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            displayTextBlock.Text = " ";
            PlaceOrder(order);
            DisplayOrderDetail(order);
        }
        private void DisplayOrderDetail(List<OrderItem> myOrder)
        {
            int total = 0, sellPrice = 0;
            string message;
            //displayTextBlock.Text = "";
            displayTextBlock.Inlines.Add(new Run("您訂購的飲品"));
            displayTextBlock.Inlines.Add(new Bold(new Run($"{takeout}")));
            displayTextBlock.Inlines.Add(new Run("，訂購明細如下 \n"));
            int i = 1;
            foreach (OrderItem item in myOrder)
            {
                total += item.SubTotal;
                Drink drinkItem = drinks[item.Index];
                //displayTextBlock.Text += $" 訂購品項{i} : {drinkItem.Name}{drinkItem.Size}，單價{ drinkItem.Price}元 X { item.Quantity}，小計{ item.SubTotal}元。\n";
            displayTextBlock.Inlines.Add(new Run($" 訂購品項{i} : {drinkItem.Name}{drinkItem.Size}，單價{ drinkItem.Price }元 X { item.Quantity}，小計{ item.SubTotal}元。\n")); 
            i++;
            }
            if (total >= 500)
            {
                sellPrice = Convert.ToInt32(Math.Round(Convert.ToDouble(total) * 0.8));
                message = "訂單總價超過500元，打8折";
            }
            else if (total >= 300)
            {
                sellPrice = Convert.ToInt32(Math.Round(Convert.ToDouble(total) * 0.85));
                message = "訂單總價超過300元，打85折";
            }
            else if (total >= 200)
            {
                sellPrice = Convert.ToInt32(Math.Round(Convert.ToDouble(total) * 0.9));
                message = "訂單總價超過200元，打9折";
            }
            else
            {
                sellPrice = total;
                message = "訂購未滿200元，沒折價";
            }
            displayTextBlock.Inlines.Add(new Italic(new Run($"訂購總價{total}元，{message}，售價{sellPrice}元。\n")));
            PlaceOrder(displayTextBlock.Text);
        }

        private void PlaceOrder(string text)
        {//throw new NotImplementedException();
            SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();

            dialog.Title = "儲存內容";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "文字檔|*.txt";

            if(dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                File.WriteAllText(path, text,Encoding.Default);
            }
            MessageBox.Show("訂單內容以存檔","訂單存檔");
        }

        private void PlaceOrder(List<OrderItem> myOrder)
        {
            myOrder.Clear();
            for (int i = 0; i < StackPanel_DrinkMenu.Children.Count; i++)
            {
                StackPanel sp = StackPanel_DrinkMenu.Children[i] as StackPanel;
                CheckBox cb = sp.Children[0] as CheckBox;
                Slider sl = sp.Children[1] as Slider;
                int Quantity = Convert.ToInt32(sl.Value);
                if (cb.IsChecked == true && Quantity != 0)
                {
                    int price = drinks[i].Price;
                    int subtotal = price * Quantity;
                    myOrder.Add(new OrderItem() { Index = i, Quantity = Quantity, SubTotal = subtotal });
                }
            }
        }
    }
}
