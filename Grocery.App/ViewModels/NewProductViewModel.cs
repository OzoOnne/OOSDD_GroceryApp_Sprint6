using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels;

public partial class NewProductViewModel : BaseViewModel
{
    private readonly IProductService _productService;
    
    [ObservableProperty]
    private string name, stock, date, price, errorMessage;

    public NewProductViewModel(IProductService productService)
    {
        _productService = productService;
    }

    [RelayCommand]
    public async Task AddProduct()
    {
        if (string.IsNullOrWhiteSpace(name))
            errorMessage = "Naam mag niet leeg zijn.";
        else if (_productService.GetAll().Exists(p => p.Name == name))
            errorMessage = "Naam bestaat al voor een ander product.";
        else if (!int.TryParse(stock, out int stockValue))
            errorMessage = "Voorraad ongeldig (verwacht: geheel getal).";
        else if (stockValue < 0)
            errorMessage = "Voorraad mag niet negatief zijn.";
        else if (!DateOnly.TryParse(date, out DateOnly dateValue))
            errorMessage = "THT-datum ongeldig (verwacht: YYYY-MM-DD).";
        else if (dateValue < DateOnly.FromDateTime(DateTime.Now))
            errorMessage = "THT-datum kan niet in het verleden liggen.";
        else if (!decimal.TryParse(price, out decimal priceValue))
            errorMessage = "Prijs ongeldig (verwacht: decimaal getal).";
        else
        {
            try
            {
                var newProduct = new Product(_productService.GetAll().Count + 1, name, stockValue, dateValue, priceValue);
                _productService.Add(newProduct);
                await Shell.Current.GoToAsync(nameof(ProductView));
            }
            catch (Exception ex)
            {
                errorMessage = $"Toevoegen mislukt: {ex.Message}";
            }
        }
    }
}