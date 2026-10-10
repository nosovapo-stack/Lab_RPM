using Lab3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lab3
{
    public partial class MainWindow : Window
    {
        private List<ProductCard> _allProducts = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                using var db = new ShopContext();

                var products = db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Supplier)
                    .Include(p => p.Name)
                    .ToList();

                _allProducts = products.Select(p => new ProductCard
                {
                    Id = p.Id,
                    Article = p.Article ?? "",
                    ProductName = p.Name?.Name ?? "Без названия",
                    CategoryName = p.Category?.Name ?? "Не указана",
                    Description = p.Description ?? "",
                    ManufacturerName = p.Manufacturer?.Name ?? "Не указан",
                    SupplierName = p.Supplier?.Name ?? "Не указан",
                    Price = p.Price ?? 0,
                    Count = p.Count ?? 0,
                    Discount = p.Discount ?? 0
                }).ToList();

                ApplySearch();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось загрузить товары из базы данных.\n\n" + ex.Message,
                    "Ошибка подключения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ApplySearch()
        {
            string search = SearchBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(search))
            {
                ProductsList.ItemsSource = _allProducts;
                return;
            }

            ProductsList.ItemsSource = _allProducts.Where(p =>
                p.Article.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.ProductName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.CategoryName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.ManufacturerName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.SupplierName.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        private void SearchBox_TextChanged(
            object sender, TextChangedEventArgs e)
        {
            if (ProductsList != null && SearchBox != null)
            {
                ApplySearch();
            }
        }

        private void AddProductButton_Click(
            object sender, RoutedEventArgs e)
        {
            var window = new ProductEditWindow();
            window.Owner = this;

            if (window.ShowDialog() == true)
            {
                LoadProducts();
            }
        }

        private void ProductCard_MouseLeftButtonUp(
            object sender, MouseButtonEventArgs e)
        {
            if (sender is not Border border ||
                border.DataContext is not ProductCard card)
            {
                return;
            }

            var window = new ProductEditWindow(
                new Product { Id = card.Id });

            window.Owner = this;

            if (window.ShowDialog() == true)
            {
                LoadProducts();
            }
        }
    }

    public class ProductCard
    {
        public int Id { get; set; }

        public string Article { get; set; } = "";
        public string ProductName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string Description { get; set; } = "";
        public string ManufacturerName { get; set; } = "";
        public string SupplierName { get; set; } = "";

        public double Price { get; set; }
        public double Count { get; set; }
        public double Discount { get; set; }

        public bool HasHighDiscount => Discount > 20;
    }
}
