using Lopushok.DB;
using Lopushok.Properties;
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

namespace Lopushok
{
    /// <summary>
    /// Interaction logic for ProductsListPage.xaml
    /// </summary>
    public partial class ProductsListPage : Page
    {
        public List<Product> Products { get; set; }
        public List<Product> FilteredProducts { get; set; }
        public List<ProductType> TypeOfProduct { get; set; }
        public Dictionary<string, int> SortDictionary { get; set; }

        public ProductsListPage()
        {
            InitializeComponent();
            Products = DBConnection.connection.Product.ToList();
            FilteredProducts = Products.ToList();
            TypeOfProduct = DBConnection.connection.ProductType.ToList();
            TypeOfProduct.Insert(0, new ProductType() { Name = "Все типы" });

            SortDictionary = new Dictionary<string, int>
            {
                { "Без сортировки", 1},
                { "Минимальная стоимость по убыванию", 2},   
                { "Минимальная стоимость по возрастанию", 3 },
                { "Номер цеха по убыванию", 4},            
                { "Номер цеха по возрастанию", 5 },
                { "Наименование по убыванию", 6},                
                { "Наименование по возрастанию", 7 }
            };

            this.DataContext = this;
        }


        private void ApplyFilters(bool filtersChanged = true)
        {
            var searchingText = tbSearch.Text.ToLower();
            var sorting = SortDictionary[cbSorting.SelectedItem as string];
            var productType = cbProductType.SelectedItem as ProductType;

            if (sorting == null || productType == null)
                return;

            FilteredProducts = Products.FindAll(p => p.Name.ToLower().Contains(searchingText));

            if (productType.Id != 0)
                FilteredProducts = FilteredProducts.FindAll(p => p.ProductType == productType);

            switch (sorting)
            {
                case 1:
                    FilteredProducts = FilteredProducts.OrderBy(p => p.Id).ToList();
                    break;

                case 2:
                    FilteredProducts = FilteredProducts.OrderByDescending(p => p.MinPrice).ToList();
                    break;

                case 3:
                    FilteredProducts = FilteredProducts.OrderBy(p => p.MinPrice).ToList();
                    break;

                case 4:
                    FilteredProducts = FilteredProducts.OrderByDescending(p => p.WorkshopId).ToList();
                    break;

                case 5:
                    FilteredProducts = FilteredProducts.OrderBy(p => p.WorkshopId).ToList();
                    break;

                case 6:
                    FilteredProducts = FilteredProducts.OrderByDescending(p => p.Name).ToList();
                    break;

                case 7:
                    FilteredProducts = FilteredProducts.OrderBy(p => p.Name).ToList();
                    break;

                default:
                    break;
            }

            lvProducts.ItemsSource = FilteredProducts;
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductPage(new Product(), true));
        }

        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvProducts.SelectedItem as Product == null)
                return;
            NavigationService.Navigate(new ProductPage(lvProducts.SelectedItem as Product, true));
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbProductType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}
