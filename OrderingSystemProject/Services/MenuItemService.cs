using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Services;

public class MenuItemService : IMenuItemService
{
    private IMenuItemsRepository _menuItemRepository;
    List<OrderItem> cart = new List<OrderItem>();
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

    //my COOOOOOOOOOOOOOOODDDDDDDDDDDDDDEEEEEEEEEEEEEEE

    public void AddItem(int itemId)
    {
        var menuItem = _menuItemRepository.GetById(itemId);

        if (menuItem == null)
        {
            throw new Exception("Menu item not found.");
        }

        // Checker
        foreach (var item in cart)
        {
            if (item.MenuItemId == itemId)
            {
                item.Amount++; 
                return;
            }
        }

        // Adding Item to Order
        cart.Add(new OrderItem
        {
            MenuItemId = itemId,
            MenuItem = menuItem,
            Amount = 1,
            Comment = "",
            ItemStatus = OrderItemStatus.NewItem
        });
    }

    public void UpdateComment(int itemId, string comment)
    {
        OrderItem orderItem = null;

        foreach (var item in cart)
        {
            if (item.MenuItemId == itemId)
            {
                orderItem = item;
                break;
            }
        }

        if (orderItem != null)
        {
            orderItem.Comment = comment;
        }
        else
        {
            throw new Exception("Item not found in cart.");
        }
    }


    public void IncreaseQuantity(int itemId)
    {
        foreach (var item in cart)
        {
            if (item.MenuItemId == itemId)
            {
                item.Amount++;
                return;
            }
        }
    }

    public void DecreaseQuantity(int itemId)
    {
        OrderItem decrQ = null;

        foreach (var item in cart)
        {
            if (item.MenuItemId == itemId)
            {
                item.Amount--;

                if (item.Amount <= 0)
                {
                    decrQ = item;
                }

                break;
            }
        }

        if (decrQ != null)
        {
            cart.Remove(decrQ);
        }
    }

    public void RemoveItem(int itemId)
    {
        for (int i = 0; i < cart.Count; i++)
        {
            if (cart[i].MenuItemId == itemId)
            {
                cart.RemoveAt(i);
                break;
            }
        }
    }

    public void CancelOrder()
    {
        while (cart.Count > 0)
        {
            cart.RemoveAt(0); // Removing 1st element
        }
    }

    //Till here is my code


    public List<OrderItem> GetCart()
    {
        return new List<OrderItem>(cart);
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