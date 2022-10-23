using System;
using System.Collections.Generic;
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
                cb.Margin = new Thickness(0);
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
            myClass.Add(new Drink() { Name = "甘蔗檸檬青", Size = "大杯", Price = 60 });
            myClass.Add(new Drink() { Name = "甘蔗檸檬青", Size = "小杯", Price = 50 });
            myClass.Add(new Drink() { Name = "甘蔗牛奶", Size = "大杯", Price = 80 });
            myClass.Add(new Drink() { Name = "甘蔗牛奶", Size = "小杯", Price = 60 });
            myClass.Add(new Drink() { Name = "QQㄋㄟㄋㄟ好喝到靠杯咩噗茶", Size = "大杯", Price = 90 });
            myClass.Add(new Drink() { Name = "QQㄋㄟㄋㄟ好喝到靠杯咩噗茶", Size = "小杯", Price = 75 });
            myClass.Add(new Drink() { Name = "靠杯紅茶", Size = "大杯", Price = 25 });
            myClass.Add(new Drink() { Name = "靠杯青茶", Size = "大杯", Price = 25 });
            myClass.Add(new Drink() { Name = "靠杯綠茶", Size = "大杯", Price = 25 });

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rd = sender as RadioButton;
            if(rd.IsChecked == true)
            {
                takeout = rd.Content.ToString();
            }
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            displayTextBlock.Text = " ";
            PlaceOrder(order);
            //DisplayOrderDetail(order);
        }
        
        private void DisplayOrderDetail(List<OrderItem> myOrder)
        {
            int total = 0;
            displayTextBlock.Text = "";

            int i = 1;
            foreach (OrderItem item in myOrder)
            {
                total += item.SubTotal;
                Drink drinkItem = drinks[item.Index];
                displayTextBlock.Text += &" 訂購品項{i} : {drinkItem.Name}{drinkItem.Size}.單價{drinkItem.Price}元 X {item.Quantity}.小計{item.SubTotal}元。\n";
                i++;
            }
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
                    myOrder.Add(new OrderItem() {Index = i,Quantity = Quantity,SubTotal = subtotal});
                }
            }

        }
    }
}
