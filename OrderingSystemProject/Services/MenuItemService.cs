using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Services;

public class MenuItemService : IMenuItemService
{
    private IMenuItemsRepository _menuItemRepository;

    public MenuItemService(IMenuItemsRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public List<MenuItem> GetAll()
    {
        return _menuItemRepository.GetAll();
    }

    public MenuItem? GetById(int id)
    {
        return _menuItemRepository.GetById(id);
    }

    public void Add(MenuItem item)
    {
        _menuItemRepository.Add(item);
    }

    public void Update(MenuItem item)
    {
        _menuItemRepository.Update(item);
    }

    public void Delete(MenuItem item)
    {
        _menuItemRepository.Delete(item);
    }

    public void Activate(int menuItemId)
    {
        _menuItemRepository.Activate(menuItemId);
    }

    public void Deactivate(int menuItemId)
    {
        _menuItemRepository.Deactivate(menuItemId);
    }

    public List<MenuItem> Filter(
        MenuManagementViewModel.CategoryFilterType category,
        MenuManagementViewModel.CardFilterType card
    )
    {
        var itemCardType = GetCardType(card);
        var itemCategoryType = GetCategoryType(category);

        if (itemCardType == null && itemCategoryType == null)
        {
            return _menuItemRepository.GetAll();
        }

        if (itemCardType == null && itemCategoryType != null)
        {
            return _menuItemRepository.FilterByCategory(itemCategoryType);
        }

        if (itemCardType != null && itemCategoryType == null)
        {
            return _menuItemRepository.FilterByCard(itemCardType);
        }

        return _menuItemRepository.FilterByCategoryAndCard(itemCategoryType, itemCardType);
    }


    private ItemCard? GetCardType(MenuManagementViewModel.CardFilterType cardFilterType)
    {
        ItemCard? itemCard = null;
        if (cardFilterType == MenuManagementViewModel.CardFilterType.DINNER)
        {
            itemCard = ItemCard.DINNER;
        }
        else if (cardFilterType == MenuManagementViewModel.CardFilterType.DRINKS)
        {
            itemCard = ItemCard.DRINKS;
        }
        else if (cardFilterType == MenuManagementViewModel.CardFilterType.LUNCH)
        {
            itemCard = ItemCard.LUNCH;
        }

        return itemCard;
    }

    private ItemCategory? GetCategoryType(MenuManagementViewModel.CategoryFilterType categoryFilterType)
    {
        ItemCategory? itemCategory = null;
        if (categoryFilterType == MenuManagementViewModel.CategoryFilterType.DESERTS)
        {
            itemCategory = ItemCategory.DESERTS;
        }
        else if (categoryFilterType == MenuManagementViewModel.CategoryFilterType.ENTREMENTS)
        {
            itemCategory = ItemCategory.ENTREMENTS;
        }
        else if (categoryFilterType == MenuManagementViewModel.CategoryFilterType.MAINS)
        {
            itemCategory = ItemCategory.MAINS;
        }
        else if (categoryFilterType == MenuManagementViewModel.CategoryFilterType.STARTERS)
        {
            itemCategory = ItemCategory.STARTERS;
        }

        return itemCategory;
    }
}