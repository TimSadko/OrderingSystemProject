using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services;

public class MenuItemService: IMenuItemService
{
    
    private IMenuItemDB _menuItemRepository;

    public MenuItemService(IMenuItemDB menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public List<MenuItem> GetAll()
    {
        return _menuItemRepository.GetAll();
    }

    public MenuItem? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(MenuItem item)
    {
        _menuItemRepository.Add(item);
    }

    public void Update(MenuItem item)
    {
        throw new NotImplementedException();
    }

    public void Delete(MenuItem item)
    {
        throw new NotImplementedException();
    }

    public void Activate(MenuItem item)
    {
        throw new NotImplementedException();
    }

    public void Deactivate(MenuItem item)
    {
        throw new NotImplementedException();
    }
}