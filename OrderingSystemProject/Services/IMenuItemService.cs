using OrderingSystemProject.Models;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Services;

public interface IMenuItemService
{
    List<MenuItem> GetAll();
    MenuItem? GetById(int id);
    void Add(MenuItem item);

    //From here
    void AddItem(int itemId);
    List<OrderItem> GetCart();
    void IncreaseQuantity(int itemId);
    void DecreaseQuantity(int itemId);
    void RemoveItem(int itemId);
    void UpdateComment(int itemId, string comment);

    void CancelOrder();

    //to here is all my code
    void Update(MenuItem item);
    void Delete(MenuItem item);
    void Activate(int menuItemId);
    void Deactivate(int menuItemId);
    List<MenuItem> Filter(MenuManagementViewModel.CategoryFilterType category,
        MenuManagementViewModel.CardFilterType card);

}