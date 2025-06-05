using OrderingSystemProject.Models;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Services;

public interface IMenuItemService
{
    List<MenuItem> GetAll();
    MenuItem? GetById(int id);
    void Add(MenuItem item);
    void Update(MenuItem item);
    void Delete(MenuItem item);
    void Activate(int menuItemId);
    void Deactivate(int menuItemId);
    List<MenuItem> Filter(MenuManagementViewModel.CategoryFilterType category,
        MenuManagementViewModel.CardFilterType card);
}