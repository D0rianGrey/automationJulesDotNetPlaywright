using Microsoft.Playwright;
using System.Threading.Tasks;

namespace SauceDemoAutomation.PageObjects
{
    public class InventoryPage
    {
        private readonly IPage _page;
        private readonly ILocator _productsTitle;
        private readonly ILocator _shoppingCartIcon;
        private readonly ILocator _inventoryItems;

        public InventoryPage(IPage page)
        {
            _page = page;
            _productsTitle = _page.Locator(".title"); 
            _shoppingCartIcon = _page.Locator("#shopping_cart_container");
            _inventoryItems = _page.Locator(".inventory_item");
        }

        public async Task<bool> IsProductsTitleVisible()
        {
            return await _productsTitle.IsVisibleAsync();
        }

        public async Task<string> GetProductsTitleText()
        {
            return await _productsTitle.InnerTextAsync();
        }

        public async Task<bool> IsShoppingCartVisible()
        {
            return await _shoppingCartIcon.IsVisibleAsync();
        }

        public async Task<int> GetInventoryItemCount()
        {
            return await _inventoryItems.CountAsync();
        }
    }
}
