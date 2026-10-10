using System;
using System.Linq;
using System.Windows;
using Lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab3
{
    public partial class ProductEditWindow : Window
    {
        private readonly ShopContext _db = new ShopContext();
        private readonly Product? _product;
        private readonly bool _isEdit;

        public ProductEditWindow(Product? product = null)
        {
            InitializeComponent();

            _product = product;
            _isEdit = product != null;

            LoadLists();

            if (_isEdit)
            {
                Title = "Редактирование товара";
                IdTextBox.Text = _product!.Id.ToString();
                NameComboBox.SelectedValue = _product.NameId;
                CategoryComboBox.SelectedValue = _product.CategoryId;
                ManufacturerComboBox.SelectedValue = _product.ManufacturerId;
                SupplierComboBox.SelectedValue = _product.SupplierId;

                DescriptionTextBox.Text = _product.Description;
                PriceTextBox.Text = _product.Price?.ToString(
                    System.Globalization.CultureInfo.InvariantCulture);
                CountTextBox.Text = _product.Count?.ToString(
                    System.Globalization.CultureInfo.InvariantCulture);
                DiscountTextBox.Text = _product.Discount?.ToString(
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                Title = "Добавление товара";
                IdTextBox.Text = "Будет назначен автоматически";
                DeleteButton.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadLists()
        {
            NameComboBox.ItemsSource = _db.ProductNames
                .AsNoTracking().OrderBy(x => x.Name).ToList();

            CategoryComboBox.ItemsSource = _db.Categories
                .AsNoTracking().OrderBy(x => x.Name).ToList();

            ManufacturerComboBox.ItemsSource = _db.Manufacturers
                .AsNoTracking().OrderBy(x => x.Name).ToList();

            SupplierComboBox.ItemsSource = _db.Suppliers
                .AsNoTracking().OrderBy(x => x.Name).ToList();
        }

        private bool TryReadNumber(
            string value, out double number)
        {
            value = value.Trim().Replace(',', '.');

            return double.TryParse(
                value,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out number)
                && !double.IsNaN(number)
                && !double.IsInfinity(number);
        }

        private void SaveButton_Click(
            object sender, RoutedEventArgs e)
        {
            if (NameComboBox.SelectedValue == null ||
                CategoryComboBox.SelectedValue == null ||
                ManufacturerComboBox.SelectedValue == null ||
                SupplierComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выбери наименование, категорию, производителя и поставщика.");
                return;
            }

            if (!TryReadNumber(PriceTextBox.Text, out double price)
                || price < 0)
            {
                MessageBox.Show("Цена должна быть числом не меньше нуля.");
                return;
            }

            if (!TryReadNumber(CountTextBox.Text, out double count)
                || count < 0)
            {
                MessageBox.Show("Количество должно быть числом не меньше нуля.");
                return;
            }

            if (!TryReadNumber(DiscountTextBox.Text, out double discount)
                || discount < 0 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть от 0 до 100.");
                return;
            }

            try
            {
                Product product;

                if (_isEdit)
                {
                    product = _db.Products
                        .First(x => x.Id == _product!.Id);
                }
                else
                {
                    product = new Product
                    {
                        Id = _db.Products.Any()
                            ? _db.Products.Max(x => x.Id) + 1
                            : 1,
                        Article = "NEW" + Guid.NewGuid()
                            .ToString("N").Substring(0, 6).ToUpper(),
                        Unit = "1",
                        ImagePath = ""
                    };

                    _db.Products.Add(product);
                }

                product.NameId = (int)NameComboBox.SelectedValue;
                product.CategoryId = (int)CategoryComboBox.SelectedValue;
                product.ManufacturerId =
                    (int)ManufacturerComboBox.SelectedValue;
                product.SupplierId = (int)SupplierComboBox.SelectedValue;

                product.Description = DescriptionTextBox.Text.Trim();
                product.Price = price;
                product.Count = count;
                product.Discount = discount;

                _db.SaveChanges();

                MessageBox.Show("Товар успешно сохранён.");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось сохранить товар:\n" + ex.Message);
            }
        }

        private void DeleteButton_Click(
            object sender, RoutedEventArgs e)
        {
            if (!_isEdit || _product == null)
                return;

            var answer = MessageBox.Show(
                "Удалить выбранный товар?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (answer != MessageBoxResult.Yes)
                return;

            try
            {
                bool usedInOrder = _db.OrderArticles
                    .Any(x => x.ProductId == _product.Id);

                if (usedInOrder)
                {
                    MessageBox.Show(
                        "Нельзя удалить товар, который присутствует в заказе.");
                    return;
                }

                var product = _db.Products
                    .FirstOrDefault(x => x.Id == _product.Id);

                if (product != null)
                {
                    _db.Products.Remove(product);
                    _db.SaveChanges();
                }

                MessageBox.Show("Товар удалён.");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось удалить товар:\n" + ex.Message);
            }
        }

        private void CancelButton_Click(
            object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _db.Dispose();
            base.OnClosed(e);
        }
    }
}