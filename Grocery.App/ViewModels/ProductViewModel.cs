using Grocery.Core.Interfaces.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly IClientService _clientService;

        [ObservableProperty] private bool admin;
        public Client CurrentClient => _clientService.CurrentClient;
        public ObservableCollection<Product> Products { get; set; }

        public ProductViewModel(IProductService productService, IClientService clientService)
        {
            _productService = productService;
            _clientService = clientService;
            Products = [];
            foreach (Product p in _productService.GetAll()) Products.Add(p);
            admin = CurrentClient?.Role == Role.Admin;;
        }
        
        [RelayCommand]
        public async Task AddNewProduct()
        {
            await Shell.Current.GoToAsync(nameof(NewProductView));
        }
    }
}
