using Lopushok.DB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Lopushok
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public Product Product { get; set; }

        public List<Workshop> Workshops { get; set; }
        public List<ProductType> TypeOfProduct { get; set; }
        public List<Material> Materials{ get; set; }
        public ProductPage(Product product, bool isNewProduct)
        {
            InitializeComponent();

            Product = product;
            Workshops = DBConnection.connection.Workshop.ToList();
            TypeOfProduct = DBConnection.connection.ProductType.ToList();
            Materials = DBConnection.connection.Material.ToList();

            lvMaterials.ItemsSource = product.Materials;

            if (isNewProduct != true)
            {
                Title = "Новый продукт";
                btnDelete.Visibility = Visibility.Hidden;
            }
            else
                btnDelete.Visibility = Visibility.Visible;

            this.DataContext = this;                
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
            };

            if (fileDialog.ShowDialog().Value)
            {
                var image = File.ReadAllBytes(fileDialog.FileName);
                Product.Image = image;

                imgProduct.Source = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!DBConnection.connection.Product.ToList().Any(p => p == Product))
                    DBConnection.connection.Product.Add(Product);
                DBConnection.connection.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Введены некорректные значения", "Ошибка");
                return;
            }

            if (!DBConnection.connection.Product.ToList().Any(p => p == Product))
                DBConnection.connection.Product.Add(Product);
            DBConnection.connection.SaveChanges();
            NavigationService.GoBack();
        }

        private void cbMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var material = cbMaterial.SelectedItem as Material;
            if (material == null || Product.ProductMaterial.Any(p => p.Material == material))
                return;

            Product.ProductMaterial.Add(new ProductMaterial() { Product = Product, Material = material });

            lvMaterials.ItemsSource = Product.ProductMaterial;
            lvMaterials.Items.Refresh();
        }

        private void lvMaterials_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var material = lvMaterials.SelectedItem as ProductMaterial;

            Product.ProductMaterial.Remove(material);

            lvMaterials.ItemsSource = Product.ProductMaterial;
            lvMaterials.Items.Refresh();
        }

        private void cbMaterial_TextChanged(object sender, TextChangedEventArgs e)
        {
            cbMaterial.ItemsSource = Materials.FindAll(material => material.Name.ToLower().Contains(cbMaterial.Text.ToLower()));
            cbMaterial.IsDropDownOpen = true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы точно хотите удалить данный продукт?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
                return;
            DBConnection.connection.Product.Remove(Product);
            DBConnection.connection.SaveChanges();
            NavigationService.GoBack();
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
